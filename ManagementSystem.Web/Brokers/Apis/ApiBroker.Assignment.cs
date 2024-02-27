using ManagementSystem.Web.Models.Assignments;

namespace ManagementSystem.Web.Brokers.Apis;

public partial class ApiBroker
{
    private const string RelativeUrl = "api/Assignment";


    public async ValueTask<Assignment> PostAssignmentAsync(Assignment assignment) =>
        await this.PostAsync(RelativeUrl, assignment);

    public async ValueTask<Assignment> GetAssignmentByIdAsync(Guid id) =>
        await this.GetAsync<Assignment>($"{RelativeUrl}/{id}");

    public async ValueTask<List<Assignment>> GetAllAssignmentsAsync() =>
        await this.GetAsync<List<Assignment>>(RelativeUrl);

    public async ValueTask<Assignment> PutAssignmentAsync(Assignment assignment) =>
        await this.PutAsync(RelativeUrl, assignment);

    public async ValueTask<Assignment> DeleteAssignmentAsync(Guid id) =>
        await this.DeleteAsync<Assignment>($"{RelativeUrl}/{id}");
}