using Backend.Domain;
using Backend.Ports;

namespace Backend.Service
{
    public class LightService : ILightService
    {
        private readonly ILightRepository _lightRepository;

        // Injisere avhengigheten ILightRepository
        public LightService(ILightRepository lightRepository)
        {
            _lightRepository = lightRepository;
        }

        // Legger til en ny enhet som lagres i en json-fil
        public async Task AddDevice(Light light)
        {
            await _lightRepository.AddDevice(light);
        }

        // Sletter en enhet fra json-fil basert på enhetens ID
        public async Task DeleteDevice(int id)
        {
            await _lightRepository.DeleteDevice(id);
        }

        // Henter alle enheter fra json-filen
        public Task<List<Light>> GetAllDevices()
        {
            return _lightRepository.GetAllDevices();
        }

        // Henter en spesifikk enhet fra json-filen basert på enhetens ID
        public Task<Light> GetDeviceById(int id)
        {
            return _lightRepository.GetDeviceById(id);
        }

        // Oppdaterer status for en enhet
        public async Task UpdateDevicePaired(int id, bool paired)
        {
            await _lightRepository.UpdateDevicePaired(id, paired);
        }

        // Oppdaterer state for en enhet (på/av)
        public async Task UpdateDeviceState(int id, bool state)
        {
            await _lightRepository.UpdateDeviceState(id, state);
        }

        // Oppdaterer state for alle enheter i en gitt gruppe
        public async Task UpdateLightFromGroup(List<Device> devices, bool state)
        {
            foreach (Device device in devices)
            {
                // Oppdaterer hver enkelt enhet i gruppen til ønsket tilstand
                await _lightRepository.UpdateDevicesFromGroup(device.Id, state);
            }
        }
    }
}

