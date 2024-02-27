using ManagementSystem.Web.Models.Assignments;

namespace ManagementSystem.Web.Brokers.Apis;

public partial interface IApiBroker
{
    ValueTask<Assignment> PostAssignmentAsync(Assignment assignment);

    ValueTask<Assignment> GetAssignmentByIdAsync(Guid id);

    ValueTask<List<Assignment>> GetAllAssignmentsAsync();

    ValueTask<Assignment> PutAssignmentAsync(Assignment assignment);

    ValueTask<Assignment> DeleteAssignmentAsync(Guid id);
}