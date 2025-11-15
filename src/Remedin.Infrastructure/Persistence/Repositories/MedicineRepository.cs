using Microsoft.EntityFrameworkCore;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;

namespace Remedin.Infrastructure.Persistence;

public class MedicineRepository : IMedicineRepository
{
    private readonly RemedinDbContext _context;

    public MedicineRepository(RemedinDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Medicine medicine)
    {
        _context.Medicines.Add(medicine);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Medicine>> GetByPersonIdAsync(Guid personId)
    {
        return await _context.Medicines
            .Where(m => m.PersonId == personId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Medicine medicine)
    {
        _context.Medicines.Update(medicine);
        await _context.SaveChangesAsync();
    }

    public async Task<Medicine?> GetByIdAsync(Guid id)
    {
        return await _context.Medicines
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}
