using Remedin.Domain.Enums;

namespace Remedin.Domain.Entities;

public class Medicine
{
    public Guid Id { get; private set; }
    public Guid PersonId { get; private set; }
    public string Name { get; private set; }
    public float DosageValue { get; private set; }
    public DosageUnit DosageUnit { get; private set; } 
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; } 
    public string? Observations { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Person? Person { get; private set; }

    protected Medicine() { }

    public Medicine(
        Guid id,
        Guid personId,
        string name,
        float dosageValue,
        DosageUnit dosageUnit,
        DateOnly startDate,
        DateOnly? endDate = null,
        string? observations = null)
    {
        Id = id;
        PersonId = personId;
        Name = name;
        DosageValue = dosageValue;
        DosageUnit = dosageUnit;
        StartDate = startDate;
        EndDate = endDate;
        Observations = observations;
        CreatedAt = DateTime.UtcNow;
    }
}
