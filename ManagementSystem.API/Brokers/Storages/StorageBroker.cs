using EFxceptions;
using ManagementSystem.API.Models.Foundation.Assignments;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Brokers.Storages;

public partial class StorageBroker : EFxceptionsContext ,IStorageBroker
{
    private readonly IConfiguration configuration;

    public StorageBroker(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    public async ValueTask<T> InsertAsync<T>(T @object)
    {
        var broker = new StorageBroker(this.configuration);
        broker.Entry(@object).State = EntityState.Added;
        await broker.SaveChangesAsync();

        return @object;
    }

    public async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class
    {
        var broker = new StorageBroker(this.configuration);

        return await broker.FindAsync<T>(objectIds);
    }

    public DbSet<T> SelectAll<T>() where T : class
    {
        var broker = new StorageBroker(this.configuration);

        return broker.Set<T>();
    }

    public async ValueTask<T> UpdateAsync<T>(T @object)
    {
        var broker = new StorageBroker(this.configuration);
        broker.Entry(@object).State = EntityState.Modified;
        await broker.SaveChangesAsync();

        return @object;
    }

    public async ValueTask<T> DeleteAsync<T>(T @object)
    {
        var broker = new StorageBroker(this.configuration);
        broker.Entry(@object).State = EntityState.Deleted;
        await broker.SaveChangesAsync();

        return @object;
    }
}