using ManagementSystem.Web.Models.Assignments;

namespace ManagementSystem.Web.Services.Foundations.Assignments;

public interface IAssignmentService
{
    ValueTask<Assignment> RegisterAssignmentAsync(Assignment assignment);
    ValueTask<Assignment> GetAssignmentByIdAsync(Guid id);
    ValueTask<List<Assignment>> GetAllAssignmentsAsync();
    ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
    ValueTask<Assignment> RemoveAssignmentAsync(Guid id);
}