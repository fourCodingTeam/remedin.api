namespace Remedin.Application.DTOs.Requests
{
    public class RegisterPessoaDto
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public float? WeightKg { get; set; }
        public int? HeightCm { get; set; }
    }
}
