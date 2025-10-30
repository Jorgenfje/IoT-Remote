using Backend.Domain;
using Backend.Ports;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata;

namespace Backend.Repository
{
    public class LightRepository : ILightRepository
    {
        private readonly IJsonFileHandler<Light> _jsonFileHandler;
        private readonly string _directoryPath = "Data/Device";

        // Injiserer en JSON-håndteringsklasse
        public LightRepository(IJsonFileHandler<Light> jsonFileHandler)
        {
            _jsonFileHandler = jsonFileHandler;
        }

        // Legger til en ny enhet i filen
        public async Task AddDevice(Light light)
        {
            var filePath = GetFilePath(); // Henter filen
            if (!File.Exists(_directoryPath)) // Oppretter mappen hvis den ikke finnes
            {
                Directory.CreateDirectory(_directoryPath);
            }
            var devices = await _jsonFileHandler.ReadFromFileList(filePath); // Leser eksisterende enheter
            var maxId = devices.Any() ? devices.Max(d => d.Id) : 0; // Finner maks ID for å tilordne en unik ID
            light.Id = maxId + 1; // Øker ID-en med 1
            devices.Add(light); // Legger til den nye enheten i listen
            await _jsonFileHandler.SaveToFileList(devices, filePath); // Lagrer listen tilbake i filen
        }

        // Sletter en enhet basert på ID
        public async Task DeleteDevice(int id)
        {
            var filePath = GetFilePath();
            var devices = await _jsonFileHandler.ReadFromFileList(filePath);
            var deviceToRemove = devices.FirstOrDefault(d => d.Id == id); // Finner enheten som skal slettes
            if (deviceToRemove != null)
            {
                devices.Remove(deviceToRemove); // Fjerner enheten fra listen
                await _jsonFileHandler.SaveToFileList(devices, filePath); // Oppdaterer filen
            }
        }

        // Henter alle enheter fra filen
        public async Task<List<Light>> GetAllDevices()
        {
            var filePath = GetFilePath();
            return await _jsonFileHandler.ReadFromFileList(filePath); // Leser og returnerer listen over enheter
        }

        // Henter en spesifikk enhet basert på ID
        public async Task<Light> GetDeviceById(int id)
        {
            var filePath = GetFilePath();
            var devices = await _jsonFileHandler.ReadFromFileList(filePath);
            return devices.FirstOrDefault(d => d.Id == id); // Returnerer enheten
        }

        // Oppdaterer paired
        public async Task UpdateDevicePaired(int id, bool paired)
        {
            var filePath = GetFilePath();
            var devices = await _jsonFileHandler.ReadFromFileList(filePath);
            var device = devices.FirstOrDefault(d => d.Id == id);
            if (device != null)
            {
                device.ChangePaired(paired); // Endrer paired-statusen til enheten
                await _jsonFileHandler.SaveToFileList(devices, filePath); // Lagrer oppdateringen
                Console.WriteLine(device.Paired);
            }
        }

        // Oppdaterer state til en enhet fra en gruppe
        public async Task UpdateDevicesFromGroup(int id, bool state)
        {
            var filePath = GetFilePath();
            var devices = await _jsonFileHandler.ReadFromFileList(filePath);
            var device = devices.FirstOrDefault(d => d.Id == id);
            if (device != null)
            {
                device.ChangeOnOrOff(state); // Endrer status av/på
                await _jsonFileHandler.SaveToFileList(devices, filePath);
            }
        }

        // Oppdaterer state til en enhet
        public async Task UpdateDeviceState(int id, bool state)
        {
            var filePath = GetFilePath();
            var devices = await _jsonFileHandler.ReadFromFileList(filePath);
            var device = devices.FirstOrDefault(d => d.Id == id);
            if (device != null)
            {
                device.ChangeOnOrOff(state); // Endrer på/av til enheten
                await _jsonFileHandler.SaveToFileList(devices, filePath); // Lagrer oppdateringen
            }
        }

        // Henter filbanen for JSON-filen
        private string GetFilePath()
        {
            return Path.Combine(_directoryPath, $"device.json"); // Kombinerer mappen og filnavnet
        }
    }
}

