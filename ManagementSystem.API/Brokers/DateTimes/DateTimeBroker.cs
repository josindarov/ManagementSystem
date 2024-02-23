namespace ManagementSystem.API.Brokers.DateTimes;

public class DateTimeBroker : IDateTimeBroker
{
    public DateTimeOffset GetCurrentDateTime() => DateTimeOffset.Now;
}