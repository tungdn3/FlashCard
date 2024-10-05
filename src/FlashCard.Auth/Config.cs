using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace FlashCard.Auth;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(name: "flash-card-api", displayName: "Flash Card API")
            {
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Role,
                }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "api-swagger",
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireClientSecret = false,
                RequirePkce = false,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = 
                { 
                    "https://localhost:7002/swagger/oauth2-redirect.html",
                    "https://tdev-flash-card-api.azurewebsites.net//swagger/oauth2-redirect.html",
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "flash-card-api"
                }
            },
            new Client
            {
                ClientId = "flash-card-bff",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
            
                // where to redirect to after login
                RedirectUris = 
                { 
                    "https://localhost:7292/signin-oidc", // bff
                    "https://tdevflashcard.azurewebsites.net/signin-oidc",
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    "https://localhost:7292/signout-callback-oidc",
                    "https://tdevflashcard.azurewebsites.net/signout-callback-oidc"
                },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "flash-card-api",
                }
            }
        };
}
