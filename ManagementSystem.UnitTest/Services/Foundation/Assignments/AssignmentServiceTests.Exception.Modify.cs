using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
    {
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Assignment someAssignment = CreateRandomAssignment(randomDateTime);
        SqlException sqlException = GetSqlException();

        FailedAssignmentStorageException failedAssignmentStorageException =
            new FailedAssignmentStorageException(sqlException);
        
        var expectedAssignmentDependencyException =
            new AssignmentDependencyException(failedAssignmentStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(sqlException);

        this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(randomDateTime);

        // when
        ValueTask<Assignment> modifyAssignmentTask =
            this.assignmentService.ModifyAssignmentAsync(someAssignment);

        AssignmentDependencyException actualAssignmentDependencyException = await Assert
            .ThrowsAsync<AssignmentDependencyException>(() =>
            modifyAssignmentTask.AsTask());
        
        // then
        actualAssignmentDependencyException.Should().BeEquivalentTo(expectedAssignmentDependencyException);
        
        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Assignment someAssignment = CreateRandomAssignment(randomDateTime);
        var serviceException = new Exception();

        var failedAssignmentServiceException =
            new FailedAssignmentServiceException(serviceException);

        var expectedAssignmentServiceException =
            new AssignmentServiceException(failedAssignmentServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(serviceException);

        this.dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime())
                .Returns(randomDateTime);

        // when
        ValueTask<Assignment> modifyAssignmentTask =
            this.assignmentService.ModifyAssignmentAsync(someAssignment);

        AssignmentServiceException actualAssignmentServiceException = await Assert
            .ThrowsAsync<AssignmentServiceException>(() =>
            modifyAssignmentTask.AsTask());
        
        // then
        actualAssignmentServiceException.Should().BeEquivalentTo(expectedAssignmentServiceException);

        this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
                Times.Never);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
                Times.Once);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedAssignmentServiceException))),
                Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

}