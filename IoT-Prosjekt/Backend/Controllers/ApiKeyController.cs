using Backend.Ports;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class ApiKeyController : Controller
    {
        private readonly IApiKeyService _apiKeyService;

        // Injisere avhengigheten IApiKeyService
        public ApiKeyController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        // Endepunkt for å registrere en ny fjernkontroll basert på remoteId
        [HttpPost("{remoteId}/register")]
        public async Task<IActionResult> RegisterNewRemote(string remoteId)
        {
            // Sjekker om en fjernkontroll med denne ID-en allerede er registrert
            var check = await _apiKeyService.CheckIfExisting(remoteId);

            if (check == true)
            {
                // Returnerer en feilmelding hvis fjernkontrollen allerede er registrert
                return BadRequest("Allerede registrert");
            }

            // Oppretter en ny API-nøkkel for fjernkontrollen
            var apiKey = await _apiKeyService.CreateApiKey(remoteId);

            // Returnerer den nye API-nøkkelen
            return Ok(apiKey);
        }
    }
}

