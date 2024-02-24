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

        var failedAssignmentStorageException =
            new FailedAssignmentStorageException(sqlException);
        
        var expectedAssignmentDependencyException =
            new AssignmentDependencyException(failedAssignmentStorageException);

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
            Times.Never);

        this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentsAsync(
                    It.IsAny<Assignment>()),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Assignment someAssignment = CreateRandomAssignment(dateTime);
        var serviceException = new Exception();

        var failedAssignmentServiceException =
            new FailedAssignmentServiceException(serviceException);

        var expectedAssignmentServiceException =
            new AssignmentServiceException(failedAssignmentServiceException);

        this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentsAsync(
                    It.IsAny<Assignment>()))
            .ThrowsAsync(serviceException);

        // when
        ValueTask<Assignment> createAssignmentTask =
            this.assignmentService.CreateAssignmentsAsync(someAssignment);

        // then
        await Assert.ThrowsAsync<AssignmentServiceException>(() =>
            createAssignmentTask.AsTask());

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentsAsync(
                    It.IsAny<Assignment>()),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentServiceException))),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}