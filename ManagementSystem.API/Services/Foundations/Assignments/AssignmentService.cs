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

    public IQueryable<Assignment> RetrieveAllAssignment() =>
        TryCatch(() =>
        {
            return this.storageBroker.SelectAllAssignments();
        });

    public ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateAssignmentId(id);
            
            Assignment assignment = await this.storageBroker
                .SelectAssignmentsByIdAsync(id);
            
            ValidateStoreAssignment(assignment, id);
            return assignment;
        });

    public ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnModify(assignment);
            
            Assignment maybeAssignment = await this.storageBroker
                .SelectAssignmentsByIdAsync(assignment.Id);
            
            ValidateStoreAssignment(maybeAssignment, assignment.Id);
            return await this.storageBroker.UpdateAssignmentsAsync(assignment);
        });

    public async ValueTask<Assignment> RemoveAssignmentAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}