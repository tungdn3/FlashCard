using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(o =>
    {
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        o.Cookie.SameSite = SameSiteMode.Strict;
        o.Cookie.HttpOnly = true;
    })
    .AddOpenIdConnect("oidc", options => ConfigureOpenIdConnect(options));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("/index.html");

app.Run();

void ConfigureOpenIdConnect(OpenIdConnectOptions options)
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
    options.ClientSecret = builder.Configuration["Auth:ClientSecret"];
    options.CallbackPath = new PathString(builder.Configuration["Auth:CallbackPath"]);

    options.ResponseType = OpenIdConnectResponseType.Code;

    // Configure the scope
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    //options.Scope.Add("offline_access");
    options.Scope.Add("flash-card-api");
    options.GetClaimsFromUserInfoEndpoint = true;

    // This saves the tokens in the session cookie
    options.SaveTokens = true;

    options.Events = new OpenIdConnectEvents
    {
        //// handle the logout redirection
        //OnRedirectToIdentityProviderForSignOut = (context) =>
        //{
        //    var logoutUri = $"{app.Configuration["Auth:Authority"]}/Account/logout?client_id={app.Configuration["Auth:ClientId"]}";

        //    var postLogoutUri = context.Properties.RedirectUri;
        //    if (!string.IsNullOrEmpty(postLogoutUri))
        //    {
        //        if (postLogoutUri.StartsWith("/"))
        //        {
        //            // transform to absolute
        //            var request = context.Request;
        //            postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
        //        }
        //        logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
        //    }
        //    context.Response.Redirect(logoutUri);
        //    context.HandleResponse();

        //    return Task.CompletedTask;
        //},
        //OnRedirectToIdentityProvider = context =>
        //{
        //    context.ProtocolMessage.SetParameter("audience", builder.Configuration["Auth:ApiAudience"]);
        //    return Task.CompletedTask;
        //}
    };
}