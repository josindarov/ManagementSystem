using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(dateTime);
        Assignment inputAssignment = randomAssignment;
        var sqlException = GetSqlException();

        var expectedAssignmentDependencyException =
            new AssignmentDependencyException(sqlException);

        this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentsAsync(
                    It.IsAny<Assignment>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<Assignment> createAssignmentTask =
            this.assignmentService.CreateAssignmentsAsync(inputAssignment);

        // then
        await Assert.ThrowsAsync<AssignmentDependencyException>(() =>
            createAssignmentTask.AsTask());

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentsAsync(
                    It.IsAny<Assignment>()),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAssignmentDependencyException))),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}