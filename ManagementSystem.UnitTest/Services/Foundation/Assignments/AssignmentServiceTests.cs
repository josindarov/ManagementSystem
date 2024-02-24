using ManagementSystem.API.Brokers.DateTimes;
using ManagementSystem.API.Brokers.Loggings;
using ManagementSystem.API.Brokers.Storages;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Services.Foundations.Assignments;
using Moq;
using Tynamix.ObjectFiller;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
    private readonly IAssignmentService assignmentService;

    public AssignmentServiceTests()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();
        this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

        this.assignmentService = new AssignmentService(
            storageBroker: storageBrokerMock.Object,
            loggingBroker: loggingBrokerMock.Object,
            dateTimeBroker: dateTimeBrokerMock.Object
        );
    }
    
    private static DateTimeOffset GetRandomDateTime() =>
        new DateTimeRange(earliestDate: new DateTime()).GetValue();
    
    private static Assignment CreateRandomAssignment(DateTimeOffset dates) =>
        CreateAssignmentFiller(dates).Create();
    private static Filler<Assignment> CreateAssignmentFiller(DateTimeOffset dates)
    {
        var filler = new Filler<Assignment>();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(dates);

        return filler;
    }
}