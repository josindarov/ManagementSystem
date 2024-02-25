using FluentAssertions;
using Force.DeepCloner;
using ManagementSystem.API.Models.Foundation.Assignments;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldModifyAssignmentAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
        Assignment inputAssignment = randomAssignment;
        Assignment storageAssignment = inputAssignment.DeepClone();
        Assignment updatedAssignment = inputAssignment;
        Assignment expectedAssignment = updatedAssignment.DeepClone();
        Guid assignmentId = inputAssignment.Id;

        this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(randomDateTime);
        
        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(assignmentId))
            .ReturnsAsync(storageAssignment);

        this.storageBrokerMock.Setup(broker =>
                broker.UpdateAssignmentsAsync(inputAssignment))
            .ReturnsAsync(updatedAssignment);

        // when 
        Assignment actualAssignment = await this.assignmentService
            .ModifyAssignmentAsync(inputAssignment);

        // then
        actualAssignment.Should().BeEquivalentTo(expectedAssignment);
        
        this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(), Times.Never);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectAssignmentsByIdAsync(assignmentId), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.UpdateAssignmentsAsync(inputAssignment), Times.Once);
        
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}