using EFxceptions.Models.Exceptions;
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
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]

    public async Task ShouldThrowValidationExceptionOnAddIfAssignmentInvalidAndLogItAsync(string invalidText)
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
        ValueTask<Assignment> addAssignmentTask = this.assignmentService
            .CreateAssignmentsAsync(invalidAssignment);

        AssignmentValidationException actualAssignmentValidationException =
            await Assert.ThrowsAsync<AssignmentValidationException>(addAssignmentTask.AsTask);
        
        // then
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
    public async Task ShouldThrowValidationExceptionOnAddIfAssignmentAlreadyExistsAndLogItAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(dateTime);
        Assignment alreadyExistsAssignment = randomAssignment;
        string randomMessage = GetRandomMessage();
        string exceptionMessage = randomMessage;
        DuplicateKeyException duplicateKeyException = new DuplicateKeyException(exceptionMessage);

        var alreadyExistsAssignmentException =
            new AlreadyExistsAssignmentException(duplicateKeyException);

        var expectedAssignmentValidationException =
            new AssignmentValidationException(alreadyExistsAssignmentException);
        
        this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentsAsync(alreadyExistsAssignment))
            .ThrowsAsync(duplicateKeyException);
        
        // when
        ValueTask<Assignment> addAssignmentTask = 
            assignmentService.CreateAssignmentsAsync(alreadyExistsAssignment);

        var actualAssignmentValidationException =
            await Assert.ThrowsAsync<AssignmentValidationException>(addAssignmentTask.AsTask);
        
        // then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.dateTimeBrokerMock.Verify(broker => 
            broker.GetCurrentDateTime(), Times.Never);
        
        this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentsAsync(alreadyExistsAssignment),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();

    }
}