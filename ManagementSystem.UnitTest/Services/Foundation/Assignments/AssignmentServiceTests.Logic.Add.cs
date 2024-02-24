using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async void ShouldAddAssignmentAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        DateTimeOffset dateTime = randomDateTime;
        Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
        Assignment inputAssignment = randomAssignment;
        Assignment storageAssignment = randomAssignment;
        Assignment expectedAssignment = storageAssignment;

        this.dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime()).Returns(dateTime);

        this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentsAsync(inputAssignment))
            .ReturnsAsync(storageAssignment);
        
        // when
        Assignment actualAssignment = await this.assignmentService.CreateAssignmentsAsync(inputAssignment);
        
        // then
        actualAssignment.Should().BeEquivalentTo(expectedAssignment);
        
        this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentsAsync(inputAssignment),
            Times.Once);

        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}