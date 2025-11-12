using Remedin.Domain.Entities;

namespace Remedin.Domain.Interfaces;

public interface IPersonRepository
{
    Task<Person?> GetBySupabaseUserIdAsync(string supabaseUserId);
    Task AddAsync(Person person);
    Task SaveChangesAsync();
}
