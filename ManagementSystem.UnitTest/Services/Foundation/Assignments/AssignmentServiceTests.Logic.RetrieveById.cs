using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAssignmentByIdAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Assignment randomAssignment = CreateRandomAssignment(dateTime);
        Guid inputAssignmentId = randomAssignment.Id;
        Assignment storedAssignment = randomAssignment;
        Assignment expectedAssignment = storedAssignment;

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentsByIdAsync(inputAssignmentId))
            .ReturnsAsync(storedAssignment);
        
        // when
        Assignment actualAssignment = await assignmentService
            .RetrieveAssignmentByIdAsync(inputAssignmentId);
        
        // then
        actualAssignment.Should().BeEquivalentTo(expectedAssignment);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectAssignmentsByIdAsync(inputAssignmentId), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}