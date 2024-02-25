using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldRemoveAssignmentAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(dateTime);
        Guid inputAssignmentId = randomAssignment.Id;
        Assignment inputAssignment = randomAssignment;
        Assignment storageAssignment = inputAssignment;
        Assignment expectedAssignment = storageAssignment;

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(inputAssignmentId))
            .ReturnsAsync(inputAssignment);

        this.storageBrokerMock.Setup(broker =>
                broker.DeleteAssignmentsAsync(inputAssignment))
            .ReturnsAsync(storageAssignment);

        // when
        Assignment actualAssignment =
            await this.assignmentService.RemoveAssignmentAsync(inputAssignmentId);

        // then
        actualAssignment.Should().BeEquivalentTo(expectedAssignment);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentsByIdAsync(inputAssignmentId),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentsAsync(inputAssignment),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}