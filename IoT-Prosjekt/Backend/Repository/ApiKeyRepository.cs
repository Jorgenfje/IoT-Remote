using Backend.Domain;
using Backend.Ports;

namespace Backend.Repository
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly IJsonFileHandler<ApiKey> _jsonFileHandler;
        private readonly string _directoryPath = "ApiKeys";

        public ApiKeyRepository(IJsonFileHandler<ApiKey> jsonFileHandler)
        {
            _jsonFileHandler = jsonFileHandler;
        }

        // Sletter en API-nøkkel basert på remoteId
        public async Task DeleteApiKey(string remoteId)
        {
            var filePath = GetFilePath(remoteId); // Finner filen for gitt remoteId
            if (File.Exists(filePath)) // Sjekker om filen eksisterer
            {
                File.Delete(filePath); // Sletter filen
            }
        }

        // Henter en API-nøkkel basert på remoteId
        public async Task<ApiKey> GetApiKeyByRemoteId(string remoteId)
        {
            var filePath = GetFilePath(remoteId); // Finner filen for gitt remoteId

            if (File.Exists(filePath)) // Sjekker om filen eksisterer
            {
                return await _jsonFileHandler.ReadFromFile(filePath); // Returnerer API-nøkkelen
            }
            return null; // Returnerer null hvis filen ikke finnes
        }

        // Lagrer en API-nøkkel
        public async Task SaveApiKey(ApiKey apiKey)
        {
            if (!File.Exists(_directoryPath)) // Sjekker om mappen eksisterer
            {
                Directory.CreateDirectory(_directoryPath); // Oppretter mappen hvis den ikke finnes
            }
            var filePath = GetFilePath(apiKey.RemoteId); // Finner filen basert på RemoteId
            await _jsonFileHandler.SaveToFile(apiKey, filePath); // Lagrer API-nøkkelen til fil
        }

        // Genererer filstien for en API-nøkkel basert på RemoteId
        private string GetFilePath(string remoteId)
        {
            return Path.Combine(_directoryPath, $"{remoteId}_ApiKey.json"); // Kombinerer katalogsti og filnavn
        }
    }
}

