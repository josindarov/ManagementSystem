using ManagementSystem.API.Models.Foundation.Assignments;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Assignment> Assignments { get; set; }

    public async ValueTask<Assignment> InsertAssignmentsAsync(Assignment assignment) =>
        await InsertAsync(assignment);

    public IQueryable<Assignment> SelectAllAssignmentsAsync() =>
        SelectAll<Assignment>();

    public async ValueTask<Assignment> SelectAssignmentsByIdAsync(Guid id) =>
        await SelectAsync<Assignment>(id);

    public async ValueTask<Assignment> UpdateAssignmentsAsync(Assignment assignment) =>
        await UpdateAsync(assignment);

    public async ValueTask<Assignment> DeleteAssignmentsAsync(Assignment assignment) =>
        await DeleteAsync(assignment);
}