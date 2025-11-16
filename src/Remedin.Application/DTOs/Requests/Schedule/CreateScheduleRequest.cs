using Remedin.Domain.Enums;

namespace Remedin.Application.DTOs.Requests.Schedule;

public record CreateScheduleRequest(
    Guid MedicineId,
    TimeOnly ScheduledTime,
    FrequencyType FrequencyType,
    int PreAlarmMinutes,
    int PosAlarmMinutes,
    List<WeekDay>? WeekDays
);