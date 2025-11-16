using Microsoft.EntityFrameworkCore;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;

namespace Remedin.Infrastructure.Persistence;

public class ScheduleRepository : IScheduleRepository
{
    private readonly RemedinDbContext _context;

    public ScheduleRepository(RemedinDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Schedule schedule)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Schedule?> GetByIdAsync(Guid id)
    {
        return await _context.Schedules
            .Include(s => s.WeekDays)
            .Include(s => s.Medicine)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Schedule>> GetByMedicineIdAsync(Guid medicineId)
    {
        return await _context.Schedules
            .Where(s => s.MedicineId == medicineId)
            .Include(s => s.WeekDays)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Schedule>> GetByPersonIdAsync(Guid personId)
    {
        return await _context.Schedules
            .Include(s => s.WeekDays)
            .Include(s => s.Medicine)
            .Where(s => s.Medicine != null && s.Medicine.PersonId == personId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<(List<Schedule> Items, int TotalCount)> GetByPersonIdPagedAsync(Guid personId, int page, int pageSize)
    {
        var query = _context.Schedules
            .Include(s => s.WeekDays)
            .Include(s => s.Medicine)
            .Where(s => s.Medicine != null && s.Medicine.PersonId == personId)
            .OrderByDescending(s => s.CreatedAt);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}