using ManagementSystem.API.Models.Foundation.Assignments;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Assignments> Assignments { get; set; }
    
    public async ValueTask<Assignments> InsertAssignmentsAsync(Assignments assignments)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignments> SelectAllAssignmentsAsync()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignments> SelectAssignmentsByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignments> UpdateAssignmentsAsync(Assignments assignments)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Assignments> DeleteAssignmentsAsync(Assignments assignments)
    {
        throw new NotImplementedException();
    }
}