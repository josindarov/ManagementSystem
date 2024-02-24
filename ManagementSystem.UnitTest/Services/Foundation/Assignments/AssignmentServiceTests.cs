using System.Linq.Expressions;
using System.Runtime.Serialization;
using ManagementSystem.API.Brokers.DateTimes;
using ManagementSystem.API.Brokers.Loggings;
using ManagementSystem.API.Brokers.Storages;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Services.Foundations.Assignments;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

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
    
    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
        actualException => actualException.SameExceptionAs(expectedException);
    
    private static string GetRandomMessage() => new MnemonicString().GetValue();
    private static SqlException GetSqlException() =>
        (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    
    private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();
    private static IQueryable<Assignment> CreateRandomAssignments(DateTimeOffset dates) =>
        CreateAssignmentFiller(dates).Create(GetRandomNumber()).AsQueryable();
}