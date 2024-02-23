using ManagementSystem.API.Models.Foundation.Assignments;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Assignments> InsertAssignmentsAsync(Assignments assignments);
    IQueryable<Assignments> SelectAllAssignmentsAsync();
    ValueTask<Assignments> SelectAssignmentsByIdAsync(Guid id);
    ValueTask<Assignments> UpdateAssignmentsAsync(Assignments assignments);
    ValueTask<Assignments> DeleteAssignmentsAsync(Assignments assignments);
}