using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class DeviceController : Controller
    {
        private readonly DeviceService _deviceService;

        public DeviceController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        // Viser en oversikt over alle enheter
        public async Task<IActionResult> Index()
        {
            List<Device> devices = await _deviceService.GetDevicesAsync(); // Henter listen over enheter
            return View(devices); // Returnerer enhetene til "Index"-visningen
        }

        // Oppdaterer paired for en enhet
        [HttpPost]
        public async Task<IActionResult> UpdateDevice(int id, bool paired)
        {
            await _deviceService.UpdateDevicePaired(id, paired); // Oppdaterer paired
            return RedirectToAction("Index"); // Går tilbake til "Index"-siden
        }

        // Lager en simulatorside 
        public async Task<IActionResult> LightSimulator()
        {
            List<Device> devices = await _deviceService.GetDevicesAsync(); // Henter listen over enheter
            return View(devices); // Returnerer enhetene til "LightSimulator"-visningen
        }

        // Viser skjema for å opprette en ny enhet
        public async Task<IActionResult> CreateDevice()
        {
            return View();
        }

        // Oppretter en ny enhet med det oppgitte navnet
        public async Task<IActionResult> Create(string deviceName)
        {
            await _deviceService.CreateDevice(deviceName); // Oppretter en ny enhet
            return RedirectToAction("Index"); // Går tilbake til "Index"-siden
        }

        // Oppdaterer state (på/av) for en enhet
        [HttpPost]
        public async Task<IActionResult> UpdateDeviceState(int id, bool state)
        {
            await _deviceService.UpdateDeviceState(id, state); // Oppdaterer state
            return RedirectToAction("LightSimulator"); // Går tilbake til "LightSimulator"-siden
        }
    }
}
