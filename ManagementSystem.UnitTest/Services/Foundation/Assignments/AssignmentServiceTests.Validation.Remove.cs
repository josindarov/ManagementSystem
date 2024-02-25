using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid randomAssignmentId = default;
        Guid inputAssignmentId = randomAssignmentId;

        var invalidAssignmentException = new InvalidAssignmentException();
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.Id),
            values: "Id is required");
        
        var expectedAssignmentValidationException =
            new AssignmentValidationException(invalidAssignmentException);

        // when
        ValueTask<Assignment> actualAssignmentTask =
            this.assignmentService.RemoveAssignmentAsync(inputAssignmentId);

        AssignmentValidationException actualAssignmentValidationException = await Assert
            .ThrowsAsync<AssignmentValidationException>(() => actualAssignmentTask.AsTask());
        
        // then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Never);

        this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentsAsync(It.IsAny<Assignment>()),
            Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }    
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveIfStorageAssignmentIsInvalidAndLogItAsync()
    {
        // given
        DateTimeOffset dateTimeOffset = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(dates: dateTimeOffset);
        Guid inputAssignmentId = randomAssignment.Id;
        Assignment inputAssignment = randomAssignment;
        Assignment nullStorageAssignment = null;

        var notFoundAssignmentException = new NotFoundAssignmentException(inputAssignmentId);

        var expectedAssignmentValidationException =
            new AssignmentValidationException(notFoundAssignmentException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(inputAssignmentId))!
            .ReturnsAsync(nullStorageAssignment);

        // when
        ValueTask<Assignment> actualAssignmentTask =
            this.assignmentService.RemoveAssignmentAsync(inputAssignmentId);

        AssignmentValidationException actualAssignmentValidationException = await Assert
            .ThrowsAsync<AssignmentValidationException>(() => actualAssignmentTask.AsTask());
        
        // then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(inputAssignmentId),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentsAsync(It.IsAny<Assignment>()),
            Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}