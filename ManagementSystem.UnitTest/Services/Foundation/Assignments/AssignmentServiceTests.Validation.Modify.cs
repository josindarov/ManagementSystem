using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfAssignmentIsNullAndLogItAsync()
    {
        //given
        Assignment invalidAssignment = null;
        var nullAssignmentException = new NullAssignmentException();

        var expectedAssignmentValidationException =
            new AssignmentValidationException(nullAssignmentException);

        //when
        ValueTask<Assignment> modifyAssignmentTask =
            this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

        AssignmentValidationException actualAssignmentValidationException = await Assert
            .ThrowsAsync<AssignmentValidationException>(() =>
            modifyAssignmentTask.AsTask());

        //then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
            Times.Once);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }
 
}