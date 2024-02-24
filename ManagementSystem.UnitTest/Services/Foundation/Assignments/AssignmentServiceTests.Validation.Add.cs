using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddAssignmentIsNullAndLogItAsync()
    {
        // given 
        Assignment nullAssignment = null;

        var nullAssignmentException = new NullAssignmentException();

        var expectedAssignmentException =
            new AssignmentValidationException(nullAssignmentException);
        
        // when
        ValueTask<Assignment> addAssignmentTask = assignmentService
            .CreateAssignmentsAsync(nullAssignment);

        AssignmentValidationException actualAssignmentValidationException =
            await Assert.ThrowsAsync<AssignmentValidationException>(addAssignmentTask.AsTask);
        
        // then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(
                SameExceptionAs(expectedAssignmentException))), Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}