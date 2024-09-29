using FlashCard.Core.Exceptions;
using FlashCard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FlashCard.Infrastructure.Repositories;

internal class IdentityRepository : IIdentityRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        if (_httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            throw new NotAuthenticatedException();
        }

        string userId = _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return userId;
    }
}
