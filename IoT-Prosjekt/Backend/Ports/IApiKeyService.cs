using Backend.Domain;

namespace Backend.Ports
{
    public interface IApiKeyService
    {
        Task<ApiKey> CreateApiKey(string remoteId);
        Task DeleteApiKey(string remoteId);
        Task<bool> CheckIfExisting(string remoteId);
    }
}
