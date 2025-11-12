namespace Remedin.Domain.Entities;

public class Person
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Username { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? Phone { get; private set; }
    public float? WeightKg { get; private set; }
    public int? HeightCm { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public string SupabaseUserId { get; private set; }

    protected Person() { }

    public Person(
        string supabaseUserId,
        string email,
        string name,
        string username,
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
        Username = username;
        BirthDate = birthDate;
        Phone = phone;
        WeightKg = weightKg;
        HeightCm = heightCm;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string? email = null,
        string? name = null,
        string? username = null,
        DateTime? birthDate = null,
        string? phone = null,
        float? weightKg = null,
        int? heightCm = null
    )
    {
        if (!string.IsNullOrWhiteSpace(email) && email != Email)
            Email = email;

        if (!string.IsNullOrWhiteSpace(name) && name != Name)
            Name = name;

        if (!string.IsNullOrWhiteSpace(username) && username != Name)
            Username = username;

        if (birthDate.HasValue && birthDate.Value != BirthDate)
            BirthDate = birthDate.Value;

        if (!string.IsNullOrWhiteSpace(phone) && phone != Phone)
            Phone = phone;

        if (weightKg.HasValue && weightKg.Value > 0 && weightKg.Value != WeightKg)
            WeightKg = weightKg.Value;

        if (heightCm.HasValue && heightCm.Value > 0 && heightCm.Value != HeightCm)
            HeightCm = heightCm.Value;

        UpdatedAt = DateTime.UtcNow;
    }
}
