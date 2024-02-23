using ManagementSystem.API.Models.Foundation.Assignments;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Assignments> Assignments { get; set; }

    public async ValueTask<Assignments> InsertAssignmentsAsync(Assignments assignments) =>
        await InsertAsync(assignments);

    public IQueryable<Assignments> SelectAllAssignmentsAsync() =>
        SelectAll<Assignments>();

    public async ValueTask<Assignments> SelectAssignmentsByIdAsync(Guid id) =>
        await SelectAsync<Assignments>(id);

    public async ValueTask<Assignments> UpdateAssignmentsAsync(Assignments assignments) =>
        await UpdateAsync(assignments);

    public async ValueTask<Assignments> DeleteAssignmentsAsync(Assignments assignments) =>
        await DeleteAsync(assignments);
}