using ManagementSystem.API.Models.Foundation.Assignments;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public interface IAssignmentService
{
    ValueTask<Assignment> CreateAssignmentsAsync(Assignment assignment);
    IQueryable<Assignment> RetrieveAllAssignment();
    ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid id);
    ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
    ValueTask<Assignment> RemoveAssignmentAsync(Guid id);
}