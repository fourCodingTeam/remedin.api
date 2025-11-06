namespace Remedin.Domain.Interfaces;

public interface IAuthService
{
    Task<(Guid Id, string Email)?> GetCurrentUserAsync(string token);
}
