using ManagementSystem.Web.Brokers.Apis;
using ManagementSystem.Web.Brokers.DateTimes;
using ManagementSystem.Web.Brokers.Loggings;
using ManagementSystem.Web.Models.Assignments;

namespace ManagementSystem.Web.Services.Foundations.Assingments;

public class AssignmentService : IAssignmentService
{
    private readonly IApiBroker apiBroker;
    private readonly ILoggingBroker loggingBroker;
    private readonly IDateTimeBroker dateTimeBroker;

    public AssignmentService(IApiBroker apiBroker, 
        ILoggingBroker loggingBroker, 
        IDateTimeBroker dateTimeBroker)
    {
        this.apiBroker = apiBroker;
        this.loggingBroker = loggingBroker;
        this.dateTimeBroker = dateTimeBroker;
    }
    
    public async ValueTask<Assignment> CreateAssignmentAsync()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<List<Assignment>> RetrieveAllAssignmentsAsync()
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