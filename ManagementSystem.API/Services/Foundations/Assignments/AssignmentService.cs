using ManagementSystem.API.Brokers.DateTimes;
using ManagementSystem.API.Brokers.Loggings;
using ManagementSystem.API.Brokers.Storages;
using ManagementSystem.API.Models.Foundation.Assignments;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public partial class AssignmentService : IAssignmentService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;
    private readonly IDateTimeBroker dateTimeBroker;

    public AssignmentService(IStorageBroker storageBroker, 
        ILoggingBroker loggingBroker, 
        IDateTimeBroker dateTimeBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
        this.dateTimeBroker = dateTimeBroker;
    }

    public ValueTask<Assignment> CreateAssignmentsAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnAdd(assignment);
            return await this.storageBroker.InsertAssignmentsAsync(assignment);
        });

    public IQueryable<Assignment> RetrieveAllAssignment()
    {
        return this.storageBroker.SelectAllAssignments();
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