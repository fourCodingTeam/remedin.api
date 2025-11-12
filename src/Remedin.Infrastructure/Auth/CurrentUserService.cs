using Microsoft.AspNetCore.Http;
using Remedin.Domain.Interfaces;
using System.Security.Claims;

namespace Remedin.Infrastructure.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? SupabaseUserId =>
         _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

    public string? Email =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
}
