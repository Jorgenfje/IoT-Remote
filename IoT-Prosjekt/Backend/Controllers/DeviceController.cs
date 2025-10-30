using Backend.Domain;
using Backend.Dto;
using Backend.Ports;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly ILightService _lightService;

        // Injisere avhengigheten ILightService
        public DeviceController(ILightService lightService)
        {
            _lightService = lightService;
        }

        // Endepunkt for å legge til en ny enhet
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Light light)
        {
            // Lagrer enheten til device.json
            await _lightService.AddDevice(light);
            return Ok(light); // Returnerer enheten som ble lagt til
        }

        // Endepunkt for å hente alle enheter
        [HttpGet("devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            // Returnerer alle enheter som finnes
            return Ok(await _lightService.GetAllDevices());
        }

        // Endepunkt for å slette en enhet basert på ID
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // Henter alle enheter og finner enheten som matcher ID-en
            var devices = await _lightService.GetAllDevices();
            var device = devices.FirstOrDefault(d => d.Id == id);
            if (device != null)
            {
                // Hvis enheten finnes, slett den
                await _lightService.DeleteDevice(id);
                return Ok(device); // Returnerer informasjon om enheten som ble slettet
            }
            return BadRequest("Enheten finnes ikke"); // Returnerer feil hvis enheten ikke finnes
        }

        // Endepunkt for å oppdatere om en enhet er koblet til (paired)
        [HttpPost("updatePaired/{id}")]
        public async Task<IActionResult> UpdateDevicePaired(int id, [FromBody] bool paired)
        {
            // Sjekker om enheten finnes basert på ID
            var check = await _lightService.GetDeviceById(id);
            if (check != null)
            {
                // Oppdaterer paired-status for enheten
                await _lightService.UpdateDevicePaired(id, paired);
                return Ok("Alt gikk bra"); // Returnerer en melding om at det gikk bra
            }
            return BadRequest("Fant ikke enheten"); // Returnerer feil hvis enheten ikke finnes
        }

        // Endepunkt for å oppdatere tilstanden (state) til en enhet
        [HttpPost("updateState/{id}")]
        public async Task<IActionResult> UpdateDeviceState(int id, [FromBody] bool state)
        {
            // Sjekker om enheten finnes basert på ID
            var check = await _lightService.GetDeviceById(id);
            if (check != null)
            {
                // Oppdaterer tilstanden til enheten
                await _lightService.UpdateDeviceState(id, state);
                return Ok(); // Returnerer suksessmelding
            }
            return BadRequest(); // Returnerer feil hvis enheten ikke finnes
        }
    }
}

