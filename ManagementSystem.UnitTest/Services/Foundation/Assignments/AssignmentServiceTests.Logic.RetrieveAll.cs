using FluentAssertions;
using ManagementSystem.API.Models.Foundation.Assignments;
using Moq;
using Xunit;

namespace ManagementSystem.UnitTest.Services.Foundation.Assignments;

public partial class AssignmentServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllAssignmentAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        IQueryable<Assignment> randomAssignments = CreateRandomAssignments(randomDateTime);
        IQueryable<Assignment> storageAssignments = randomAssignments;
        IQueryable<Assignment> expectedAssignments = storageAssignments;

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
            .Returns(storageAssignments);

        // when
        IQueryable<Assignment> actualAssignments =
            this.assignmentService.RetrieveAllAssignment();

        // then
        actualAssignments.Should().BeEquivalentTo(expectedAssignments);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
            Times.Once);

        this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}