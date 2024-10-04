using FlashCard.Api;
using FlashCard.Core;
using FlashCard.Infrastructure;
using FlashCard.Infrastructure.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Flash Card API",
        Description = "An ASP.NET Core Web API for managing flash cards",
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{builder.Configuration["Auth:Authority"]}/connect/authorize"),
                TokenUrl = new Uri($"{builder.Configuration["Auth:Authority"]}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "flash-card-api", "Flash Card API - full access" }
                }
            }
        },
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2",
                },
            },
            Array.Empty<string>()
        }
    });
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "flash-card-api");
    });
});

builder.Services.AddCoreLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(app.Configuration["Auth:SwaggerClientId"]);
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseStatusCodePages();

app.UseAuthorization();

app.MapControllers();

SeedData();

app.Run();


// Just for local development
void SeedData()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<FlashCardDbContext>();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
}
