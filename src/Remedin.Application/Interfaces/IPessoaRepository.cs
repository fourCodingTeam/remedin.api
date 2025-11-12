using Remedin.Domain.Entities;

namespace Remedin.Application.Interfaces
{
    public interface IPessoaRepository
    {
        Task<Person?> GetBySupabaseUserIdAsync(string supabaseUserId);
        Task AddAsync(Person pessoa);
        Task SaveChangesAsync();
    }
}
