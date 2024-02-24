using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfSqlExceptionOccursAndLogIt()
    {
        // given
        Guid someAssignmentId = Guid.NewGuid();
        var sqlException = GetSqlException();

        var failedAssignmentStorageException =
            new FailedAssignmentStorageException(sqlException);
        
        var expectedAssignmentDependencyException =
            new AssignmentDependencyException(failedAssignmentStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()))
            .Throws(sqlException);

        // when 
        ValueTask<Assignment> retrieveTask =
            this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

        AssignmentDependencyException actualAssignmentDependencyException = await Assert
            .ThrowsAsync<AssignmentDependencyException>(() => retrieveTask.AsTask());
        
        // then
        actualAssignmentDependencyException.Should().BeEquivalentTo(expectedAssignmentDependencyException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfExceptionOccursAndLogIt()
    {
        // given
        Guid someAssignmentId = Guid.NewGuid();
        var serviceException = new Exception();

        var failedAssignmentServiceException =
            new FailedAssignmentServiceException(serviceException);

        var expectedAssignmentServiceException =
            new AssignmentServiceException(failedAssignmentServiceException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()))
            .Throws(serviceException);

        // when 
        ValueTask<Assignment> retrieveTask =
            this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

        AssignmentServiceException actualAssignmentServiceException = await Assert
            .ThrowsAsync<AssignmentServiceException>(() => retrieveTask.AsTask());
        
        // then
        actualAssignmentServiceException.Should().BeEquivalentTo(expectedAssignmentServiceException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentServiceException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}