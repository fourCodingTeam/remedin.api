using Microsoft.EntityFrameworkCore;
using Remedin.Domain.Entities;

namespace Remedin.Infrastructure.Persistence;

public class RemedinDbContext : DbContext
{
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Medicine> Medicines => Set<Medicine>();

    public RemedinDbContext(DbContextOptions<RemedinDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(120);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(150);
            builder.Property(p => p.SupabaseUserId).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Medicine>(builder =>
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(150);
            builder.Property(m => m.DosageValue).IsRequired();
            builder.Property(m => m.DosageUnit).IsRequired();
            builder.Property(m => m.StartDate).IsRequired();
            builder.HasOne<Person>()
                   .WithMany()
                   .HasForeignKey(m => m.PersonId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
