using ManagementSystem.Web.Brokers.Apis;
using ManagementSystem.Web.Brokers.Loggings;
using ManagementSystem.Web.Models.Assignments;

namespace ManagementSystem.Web.Services.Foundations.Assignments;

public class AssignmentService : IAssignmentService
{
    private readonly IApiBroker apiBroker;
    private readonly ILoggingBroker loggingBroker;

    public AssignmentService(IApiBroker apiBroker, 
        ILoggingBroker loggingBroker)
    {
        this.apiBroker = apiBroker;
        this.loggingBroker = loggingBroker;
    }
    
    public async ValueTask<Assignment> RegisterAssignmentAsync(Assignment assignment)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignment> GetAssignmentByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<List<Assignment>> GetAllAssignmentsAsync()
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