using Remedin.Domain.Entities;
using Remedin.Domain.Enums;

public class Schedule
{
    public Guid Id { get; private set; }
    public Guid MedicineId { get; private set; }
    public TimeOnly ScheduledTime { get; private set; }
    public FrequencyType FrequencyType { get; private set; }
    public int PreAlarmMinutes { get; private set; }
    public int PosAlarmMinutes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Medicine? Medicine { get; private set; }
    public List<ScheduleWeekDay> WeekDays { get; private set; } = new();

    protected Schedule() { }

    public Schedule(
        Guid id,
        Guid medicineId,
        TimeOnly scheduledTime,
        FrequencyType frequencyType,
        int preAlarmMinutes = 0,
        int posAlarmMinutes = 0)
    {
        Id = id;
        MedicineId = medicineId;
        ScheduledTime = scheduledTime;
        FrequencyType = frequencyType;
        PreAlarmMinutes = preAlarmMinutes;
        PosAlarmMinutes = posAlarmMinutes;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateBaseData(
    Guid medicineId,
    TimeOnly scheduledTime,
    FrequencyType frequencyType,
    int preAlarmMinutes,
    int posAlarmMinutes)
    {
        MedicineId = medicineId;
        ScheduledTime = scheduledTime;
        FrequencyType = frequencyType;
        PreAlarmMinutes = preAlarmMinutes;
        PosAlarmMinutes = posAlarmMinutes;
    }
}
