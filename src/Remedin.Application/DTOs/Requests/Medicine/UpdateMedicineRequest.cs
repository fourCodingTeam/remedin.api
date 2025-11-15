using Remedin.Domain.Enums;

namespace Remedin.Application.DTOs.Requests.Medicine;

public record UpdateMedicineRequest(
    Guid Id,
    string Name,
    float DosageValue,
    DosageUnit DosageUnit,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Observations
);
