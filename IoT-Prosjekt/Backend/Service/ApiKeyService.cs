using Backend.Domain;
using Backend.Ports;

namespace Backend.Service
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository _apiKeyRepository;

        // Injiserer repository for API-nøkler
        public ApiKeyService(IApiKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        // Sjekker om en API-nøkkel eksisterer for en gitt remoteId
        public async Task<bool> CheckIfExisting(string remoteId)
        {
            var check = await _apiKeyRepository.GetApiKeyByRemoteId(remoteId);
            return check != null; // Returnerer true hvis nøkkelen finnes, ellers false
        }

        // Oppretter en ny API-nøkkel for en gitt remoteId
        public async Task<ApiKey> CreateApiKey(string remoteId)
        {
            // Genererer en unik nøkkel ved bruk av GUID
            string apiKey = Guid.NewGuid().ToString();
            
            // Oppretter en ny ApiKey med remoteId og apiKey
            var api = new ApiKey()
            {
                RemoteId = remoteId,
                Key = apiKey
            };

            // Lagrer API-nøkkelen i en json-fil
            await _apiKeyRepository.SaveApiKey(api);
            return api; // Returnerer den opprettede nøkkelen
        }

        // Sletter en API-nøkkel basert på RemoteId
        public async Task DeleteApiKey(string remoteId)
        {
            await _apiKeyRepository.DeleteApiKey(remoteId);
        }
    }
}

