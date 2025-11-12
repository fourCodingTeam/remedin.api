namespace Remedin.Domain.Entities;

public class Person
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? Phone { get; private set; }
    public float? WeightKg { get; private set; }
    public int? HeightCm { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public string SupabaseUserId { get; private set; }

    protected Person() { }

    public Person(
        string supabaseUserId,
        string email,
        string name,
        DateTime? birthDate = null,
        string? phone = null,
        float? weightKg = null,
        int? heightCm = null
    )
    {
        Id = Guid.NewGuid();
        SupabaseUserId = supabaseUserId;
        Email = email;
        Name = name;
        BirthDate = birthDate;
        Phone = phone;
        WeightKg = weightKg;
        HeightCm = heightCm;
        CreatedAt = DateTime.UtcNow;
    }
}
