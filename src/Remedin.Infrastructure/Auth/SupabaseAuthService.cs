using Supabase.Gotrue;
using Remedin.Domain.Interfaces;

namespace Remedin.Infrastructure.Auth;

public class SupabaseAuthService : IAuthService
{
    private readonly AdminClient _adminClient;

    public SupabaseAuthService(AdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<(Guid Id, string Email)?> GetCurrentUserAsync(string token)
    {
        var user = await _adminClient.GetUser(token);
        if (user == null) return null;

        return (Guid.Parse(user.Id), user.Email!);
    }
}
