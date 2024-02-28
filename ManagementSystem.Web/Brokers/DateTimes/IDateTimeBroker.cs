namespace ManagementSystem.Web.Brokers.DateTimes;

public interface IDateTimeBroker
{
    DateTimeOffset GetCurrentDateTime();

}