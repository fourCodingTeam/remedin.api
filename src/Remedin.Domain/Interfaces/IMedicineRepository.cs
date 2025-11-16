using Remedin.Domain.Entities;

namespace Remedin.Domain.Interfaces;

public interface IMedicineRepository
{
    Task AddAsync(Medicine medicine);
    Task<(List<Medicine> Items, int TotalCount)> GetByPersonIdPagedAsync(Guid personId, int page, int pageSize);
    Task UpdateAsync(Medicine medicine);
    Task<Medicine?> GetByIdAsync(Guid id);
}
