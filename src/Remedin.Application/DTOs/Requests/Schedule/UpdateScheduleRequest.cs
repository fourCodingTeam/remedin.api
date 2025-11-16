using Remedin.Domain.Enums;

namespace Remedin.Application.DTOs.Requests.Schedule;

public record UpdateScheduleRequest(
    Guid Id,
    Guid MedicineId,
    TimeOnly ScheduledTime,
    FrequencyType FrequencyType,
    int PreAlarmMinutes,
    int PosAlarmMinutes,
    List<WeekDay>? WeekDays
);