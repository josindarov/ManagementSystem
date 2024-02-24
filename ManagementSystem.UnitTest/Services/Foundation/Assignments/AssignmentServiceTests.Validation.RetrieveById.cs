using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        var invalidAssignmentException = new InvalidAssignmentException();
        
        invalidAssignmentException.AddData(
            key: nameof(Assignment.Id),
            values: "Id is required");

        var expectedAssignmentValidationException =
            new AssignmentValidationException(invalidAssignmentException);

        // when
        ValueTask<Assignment> retrieveAssignmentByIdTask = assignmentService
            .RetrieveAssignmentByIdAsync(invalidId);

        AssignmentValidationException actualAssignmentValidationException =
            await Assert.ThrowsAsync<AssignmentValidationException>(retrieveAssignmentByIdTask.AsTask);

        // then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageAssignmentIsNullAndLogItAsync()
    {
        //given
        Guid randomAssignmentId = Guid.NewGuid();
        Guid inputAssignmentId = randomAssignmentId;
        Assignment invalidStorageAssignment = null;

        var notFoundAssignmentException = new NotFoundAssignmentException(inputAssignmentId);

        var expectedAssignmentValidationException = new AssignmentValidationException(notFoundAssignmentException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(inputAssignmentId))!
            .ReturnsAsync(invalidStorageAssignment);

        //when
        ValueTask<Assignment> retrieveAssignmentByIdTask =
            this.assignmentService.RetrieveAssignmentByIdAsync(inputAssignmentId);

        AssignmentValidationException actualAssignmentValidationException = await Assert
            .ThrowsAsync<AssignmentValidationException>(() =>
            retrieveAssignmentByIdTask.AsTask());
        
        //then
        actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
            Times.Once);

        this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
            Times.Never);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}