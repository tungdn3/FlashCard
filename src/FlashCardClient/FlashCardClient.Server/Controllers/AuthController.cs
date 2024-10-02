using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlashCardClient.Server.Controllers;

public class AuthController : Controller
{
    public ActionResult Login(string returnUrl = "/")
    {
        return new ChallengeResult(
            "oidc",
            new AuthenticationProperties()
            {
                RedirectUri = returnUrl
            });
    }

    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return new SignOutResult(
            "oidc",
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
    }

    public ActionResult<UserDto> GetUser()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            Dictionary<string, string> claimsMap = ((ClaimsIdentity)User.Identity).Claims.ToDictionary(x => x.Type, x => x.Value);

            return Json(new UserDto
            {
                Id = claimsMap[ClaimTypes.NameIdentifier],
                Name = claimsMap["name"],
            });
        }

        return Json(null);
    }
}
