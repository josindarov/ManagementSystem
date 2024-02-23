using ManagementSystem.API.Models.Foundation.Assignments;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Assignment> InsertAssignmentsAsync(Assignment assignment);
    IQueryable<Assignment> SelectAllAssignmentsAsync();
    ValueTask<Assignment> SelectAssignmentsByIdAsync(Guid id);
    ValueTask<Assignment> UpdateAssignmentsAsync(Assignment assignment);
    ValueTask<Assignment> DeleteAssignmentsAsync(Assignment assignment);
}