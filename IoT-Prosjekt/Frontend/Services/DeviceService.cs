using Frontend.Dto;
using Frontend.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Frontend.Services
{
    public class DeviceService
    {
        // HttpClient brukes til å kommunisere med backend-API
        private readonly HttpClient _httpClient;
        private readonly int _port = 5048; // Porten til backend

        public DeviceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Henter en liste over enheter fra backend
        public async Task<List<Device>> GetDevicesAsync()
        {
            var response = await _httpClient.GetAsync($"http://localhost:{_port}/api/v1/device/devices"); // Sender GET-forespørsel til API-et
            response.EnsureSuccessStatusCode();
            var devices = await response.Content.ReadFromJsonAsync<List<Device>>(); // Lagrer enhetene i devices
            return devices; // Returnerer listen over enheter
        }

        // Oppdaterer paired for en enhet
        public async Task UpdateDevicePaired(int id, bool paired)
        {
            // Sender POST-forespørsel med ny status
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:{_port}/api/v1/device/updatePaired/{id}", paired);
            response.EnsureSuccessStatusCode();
        }

        // Oppretter en ny enhet
        public async Task CreateDevice(string name)
        {
            // Lager et Dto-objekt
            var request = new CreateDeviceDto()
            {
                Name = name,
                Type = "light",
                State = false,
                Paired = false,
                Brightness = 100
            };

            var response = await _httpClient.PostAsJsonAsync($"http://localhost:{_port}/api/v1/device/add", request); // Sender POST-forespørsel for å legge til enheten
            response.EnsureSuccessStatusCode();
        }

        // Oppdaterer state (på/av) for en enhet
        public async Task UpdateDeviceState(int id, bool state)
        {
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:{_port}/api/v1/device/updateState/{id}", state); // Sender POST-forespørsel med ny state
            response.EnsureSuccessStatusCode();
        }
    }
}

