using Backend.Domain;

namespace Backend.Ports
{
    public interface IApiKeyRepository
    {
        Task<ApiKey> GetApiKeyByRemoteId(string remoteId);
        Task SaveApiKey(ApiKey apiKey);
        Task DeleteApiKey(string remoteId);
    }
}
