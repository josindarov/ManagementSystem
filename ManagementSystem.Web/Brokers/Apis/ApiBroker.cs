using ManagementSystem.Web.Models.Configurations;
using RESTFulSense.Clients;

namespace ManagementSystem.Web.Brokers.Apis;

public class ApiBroker : IApiBroker
{
    private readonly IRESTFulApiFactoryClient apiClient;
    private readonly HttpClient httpClient;

    public ApiBroker(IRESTFulApiFactoryClient apiClient, HttpClient httpClient)
    {
        this.apiClient = apiClient;
        this.httpClient = httpClient;
    }

    private async ValueTask<T> GetAsync<T>(string relativeUrl) =>
        await this.apiClient.GetContentAsync<T>(relativeUrl);

    private async ValueTask<T> PostAsync<T>(string relativeUrl, T content) =>
        await this.apiClient.PostContentAsync<T>(relativeUrl, content);

    private async ValueTask<T> PutAsync<T>(string relativeUrl, T content) =>
        await this.apiClient.PutContentAsync<T>(relativeUrl, content);

    private async ValueTask<T> DeleteAsync<T>(string relativeUrl) =>
        await this.apiClient.DeleteContentAsync<T>(relativeUrl);

    private IRESTFulApiFactoryClient GetApiClient(IConfiguration configuration)
    {
        LocalConfigurations localConfigurations =
            configuration.Get<LocalConfigurations>();

        string apiBaseUrl = localConfigurations.ApiConfigurations.Url;

        return new RESTFulApiFactoryClient(this.httpClient);
    }
}