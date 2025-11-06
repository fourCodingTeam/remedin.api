using Microsoft.EntityFrameworkCore;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;
using System;

namespace Remedin.Infrastructure.Persistence;

public class PersonRepository : IPersonRepository
{
    private readonly RemedinDbContext _context;

    public PersonRepository(RemedinDbContext context)
    {
        _context = context;
    }

    public async Task<Person?> GetBySupabaseUserIdAsync(string supabaseUserId)
    {
        return await _context.Persons.FirstOrDefaultAsync(p => p.SupabaseUserId == supabaseUserId);
    }

    public async Task AddAsync(Person person)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
    }
}
