namespace ManagementSystem.Web.Brokers.DateTimes;

public class DateTimeBroker : IDateTimeBroker
{
    public DateTimeOffset GetCurrentDateTime() 
        => DateTimeOffset.Now;    
}