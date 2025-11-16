using Remedin.Domain.Enums;

public class ScheduleWeekDay
{
    public Guid ScheduleId { get; private set; }
    public WeekDay DayOfWeek { get; private set; }

    public Schedule Schedule { get; private set; }

    protected ScheduleWeekDay() { }

    public ScheduleWeekDay(Guid scheduleId, WeekDay dayOfWeek)
    {
        ScheduleId = scheduleId;
        DayOfWeek = dayOfWeek;
    }
}
