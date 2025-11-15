using Remedin.Domain.Entities;

namespace Remedin.Domain.Interfaces;

public interface IMedicineRepository
{
    Task AddAsync(Medicine medicine);
    Task<List<Medicine>> GetByPersonIdAsync(Guid personId);
    Task UpdateAsync(Medicine medicine);
    Task<Medicine?> GetByIdAsync(Guid id);
}
