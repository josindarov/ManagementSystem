using ManagementSystem.Web.Models.Assignments;

namespace ManagementSystem.Web.Services.Foundations.Assingments;

public interface IAssignmentService
{
    ValueTask<Assignment> CreateAssignmentAsync();
    ValueTask<List<Assignment>> RetrieveAllAssignmentsAsync();
    ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid id);
    ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
    ValueTask<Assignment> RemoveAssignmentAsync(Guid id);
}