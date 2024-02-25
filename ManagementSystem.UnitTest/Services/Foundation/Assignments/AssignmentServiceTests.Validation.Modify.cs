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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnModifyIfInvalidAssignmentAndLogItAsync(string invalidText)
    {
        // given
        var invalidAssignment = new Assignment()
        {
            Title = invalidText,
            Description = invalidText,
            Note = invalidText,
            State = invalidText,
            TaskPriority = invalidText
        };

        var invalidAssignmentException = new InvalidAssignmentException();
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.Id),
            values: "Id is required");
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.Title),
            values: "Text is required");
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.Description),
            values: "Text is required");
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.Note),
            values: "Text is required");
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.TaskPriority),
            values: "Text is required");
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.State),
            values: "Text is required");
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.DueDate),
            values: "Date is required");

        var expectedAssignmentValidationException =
            new AssignmentValidationException(invalidAssignmentException);
        
        // when
        ValueTask<Assignment> modifyAssignmentTask = this.assignmentService
            .ModifyAssignmentAsync(invalidAssignment);

        AssignmentValidationException actualAssignmentValidationException =
            await Assert.ThrowsAsync<AssignmentValidationException>(modifyAssignmentTask.AsTask);
        
        // then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.dateTimeBrokerMock.Verify(broker => 
            broker.GetCurrentDateTime(), Times.Never);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(
                SameExceptionAs(expectedAssignmentValidationException))), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.InsertAssignmentsAsync(It.IsAny<Assignment>()),Times.Never);
        
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfAssignmentDoesntExistAndLogItAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(dateTime);
        Assignment nonExistentAssignment = randomAssignment;
        Assignment noAssignment = null;
        
        var notFoundAssignmentException = 
            new NotFoundAssignmentException(nonExistentAssignment.Id);

        var expectedAssignmentValidationException =
            new AssignmentValidationException(notFoundAssignmentException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(nonExistentAssignment.Id))!
                .ReturnsAsync(noAssignment);

        this.dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime())
                .Returns(dateTime);

        // when
        ValueTask<Assignment> modifyAssignmentTask =
            this.assignmentService.ModifyAssignmentAsync(nonExistentAssignment);

        AssignmentValidationException actualAssignmentValidationException = await Assert
            .ThrowsAsync<AssignmentValidationException>(() =>
            modifyAssignmentTask.AsTask());
        
        // then
        actualAssignmentValidationException.Should()
            .BeEquivalentTo(expectedAssignmentValidationException);
        
        this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
                Times.Never);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAssignmentsByIdAsync(nonExistentAssignment.Id),
                Times.Once);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                Times.Once);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}