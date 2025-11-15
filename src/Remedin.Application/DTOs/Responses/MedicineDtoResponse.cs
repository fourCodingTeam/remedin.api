using Remedin.Domain.Enums;

namespace Remedin.Application.DTOs.Responses;

public record MedicineDtoResponse(
    Guid Id,
    string Name,
    float DosageValue,
    DosageUnit DosageUnit,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Observations
);
