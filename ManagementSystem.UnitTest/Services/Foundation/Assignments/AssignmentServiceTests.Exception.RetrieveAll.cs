using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRetrieveAllAssignmentsIfSqlErrorOccursAndLogItAsync()
    {
        // given
        var sqlException = GetSqlException();

        var failedAssignmentStorageException =
            new FailedAssignmentStorageException(sqlException);
        
        var expectedAssignmentDependencyException =
            new AssignmentDependencyException(failedAssignmentStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
            .Throws(sqlException);
    
        // when
        Action retrieveAllAssignmentsAction = () =>
            this.assignmentService.RetrieveAllAssignment();

        Assert.Throws<AssignmentDependencyException>(
            retrieveAllAssignmentsAction);
        
        // then
        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAssignmentDependencyException))),
            Times.Once);

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();    
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllAssignmentsIfServiceErrorOccursAndLogItAsync()
    {
        // given
        var serviceException = new Exception();

        FailedAssignmentServiceException failedAssignmentServiceException =
            new FailedAssignmentServiceException(serviceException);

        var expectedAssignmentServiceException =
            new AssignmentServiceException(failedAssignmentServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectAllAssignments()).Throws(serviceException);
        
        Action retrieveAllAssignmentsAction = () =>
            this.assignmentService.RetrieveAllAssignment();

        Assert.Throws<AssignmentServiceException>(
            retrieveAllAssignmentsAction);

        // then
        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentServiceException))),
            Times.Once);

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}