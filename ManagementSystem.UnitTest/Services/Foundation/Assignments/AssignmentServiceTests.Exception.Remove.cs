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
    public async Task ShouldThrowDependencyExceptionOnRemoveIfSqlExceptionOccursAndLogItAsync()
    {
        // given
        Guid someAssignmentId = Guid.NewGuid();
        SqlException sqlException = GetSqlException();

        FailedAssignmentStorageException failedAssignmentStorageException =
            new FailedAssignmentStorageException(sqlException);
        
        var expectedAssignmentDependencyException =
            new AssignmentDependencyException(failedAssignmentStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(sqlException);

        // when
        ValueTask<Assignment> deleteAssignmentTask =
            this.assignmentService.RemoveAssignmentAsync(someAssignmentId);

        AssignmentDependencyException actualAssignmentDependencyException = await Assert
            .ThrowsAsync<AssignmentDependencyException>(() =>
            deleteAssignmentTask.AsTask());
        
        // then
        actualAssignmentDependencyException.Should().BeEquivalentTo(expectedAssignmentDependencyException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}