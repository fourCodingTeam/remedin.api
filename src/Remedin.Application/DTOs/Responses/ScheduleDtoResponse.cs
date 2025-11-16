using Remedin.Domain.Enums;

namespace Remedin.Application.DTOs.Responses;

public record ScheduleDtoResponse(
    Guid Id,
    Guid MedicineId,
    TimeOnly ScheduledTime,
    FrequencyType FrequencyType,
    int PreAlarmMinutes,
    int PosAlarmMinutes,
    List<WeekDay> WeekDays
);