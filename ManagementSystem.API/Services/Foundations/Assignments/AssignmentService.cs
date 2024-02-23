using ManagementSystem.API.Models.Foundation.Assignments;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public class AssignmentService : IAssignmentService
{
    public async ValueTask<Assignment> CreateAssignmentsAsync(Assignment assignment)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Assignment> RetrieveAllAssignment()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignment> RemoveAssignmentAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}