namespace Remedin.Application.Interfaces;

public interface ICurrentUserService
{
    string? SupabaseUserId { get; }
    string? Email { get; }
}

