namespace Remedin.Domain.Interfaces;

public interface ICurrentUserService
{
    string? SupabaseUserId { get; }
    string? Email { get; }
}
