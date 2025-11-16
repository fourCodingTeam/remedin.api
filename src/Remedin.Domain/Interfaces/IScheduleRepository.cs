using Remedin.Domain.Entities;

namespace Remedin.Domain.Interfaces;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task<Schedule?> GetByIdAsync(Guid id);
    Task<List<Schedule>> GetByMedicineIdAsync(Guid medicineId);
    Task<List<Schedule>> GetByPersonIdAsync(Guid personId);
    Task<(List<Schedule> Items, int TotalCount)> GetByPersonIdPagedAsync(Guid personId, int page, int pageSize);
}