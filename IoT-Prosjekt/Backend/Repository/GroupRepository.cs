using Backend.Domain;
using Backend.Ports;

namespace Backend.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IJsonFileHandler<Group> _jsonFileHandler;
        private readonly string _directoryPath = "Data/Group";

        public GroupRepository(IJsonFileHandler<Group> jsonFileHandler)
        {
            _jsonFileHandler = jsonFileHandler;
        }

        // Legger til en enhet i en eksisterende gruppe
        public async Task AddDeviceToGroup(int groupId, Device device)
        {
            var filePath = GetFilePath(); // Finner filstien
            var groups = await _jsonFileHandler.ReadFromFileList(filePath);
            var group = groups.FirstOrDefault(g => g.Id == groupId); // Finner gruppen med riktig ID
            if (group != null)
            {
                group.Devices.Add(device); // Legger til enheten i gruppen
                await _jsonFileHandler.SaveToFileList(groups, filePath); // Lagrer oppdaterte grupper tilbake til fil
            }
        }

        // Legger til en ny gruppe
        public async Task AddGroup(Group group)
        {
            var filePath = GetFilePath();
            if (!File.Exists(_directoryPath)) // Sjekker om mappen eksisterer
            {
                Directory.CreateDirectory(_directoryPath); // Oppretter mappen hvis den ikke finnes
            }
            var groups = await _jsonFileHandler.ReadFromFileList(filePath);
            var maxId = groups.Any() ? groups.Max(g => g.Id) : 0; // Finner høyeste ID
            group.Id = maxId + 1; // Setter ny ID for gruppen
            groups.Add(group); // Legger til gruppen i listen
            await _jsonFileHandler.SaveToFileList(groups, filePath); // Lagrer oppdaterte grupper tilbake til fil
        }

        // Sletter en gruppe basert på ID
        public async Task DeleteGroup(int groupId)
        {
            var filePath = GetFilePath();
            var groups = await _jsonFileHandler.ReadFromFileList(filePath);
            var groupToRemove = groups.FirstOrDefault(g => g.Id == groupId); // Finner gruppen som skal slettes
            if (groupToRemove != null)
            {
                groups.Remove(groupToRemove); // Fjerner gruppen
                await _jsonFileHandler.SaveToFileList(groups, filePath); // Oppdaterer filen
            }
        }

        // Henter alle grupper
        public async Task<List<Group>> GetAllGroups()
        {
            var filePath = GetFilePath();
            return await _jsonFileHandler.ReadFromFileList(filePath); // Returnerer listen over grupper
        }

        // Henter en gruppe basert på ID
        public async Task<Group> GetGroupById(int id)
        {
            var filePath = GetFilePath();
            var groups = await _jsonFileHandler.ReadFromFileList(filePath);
            return groups.FirstOrDefault(g => g.Id == id); // Returnerer gruppen med riktig ID
        }

        // Fjerner en enhet fra en gruppe
        public async Task RemoveDeviceFromGroup(int groupId, int deviceId)
        {
            var filePath = GetFilePath();
            var groups = await _jsonFileHandler.ReadFromFileList(filePath);
            var group = groups.FirstOrDefault(g => g.Id == groupId); // Finner gruppen
            if (group != null)
            {
                var deviceToRemove = group.Devices.FirstOrDefault(d => d.Id == deviceId); // Finner enheten som skal fjernes
                if (deviceToRemove != null)
                {
                    group.Devices.Remove(deviceToRemove); // Fjerner enheten fra gruppen
                    await _jsonFileHandler.SaveToFileList(groups, filePath); // Oppdaterer filen
                }
            }
        }

        // Oppdaterer state (på/av) for en enhet i en gruppe
        public async Task UpdateGroup(int groupId, int deviceId, bool state)
        {
            var filePath = GetFilePath();
            var groups = await _jsonFileHandler.ReadFromFileList(filePath);
            var group = groups.FirstOrDefault(g => g.Id == groupId); // Finner gruppen
            if (group != null)
            {
                var deviceToUpdate = group.Devices.FirstOrDefault(d => d.Id == deviceId); // Finner enheten som skal oppdateres
                if (deviceToUpdate != null)
                {
                    deviceToUpdate.ChangeOnOrOff(state); // Endrer status for enheten
                    await _jsonFileHandler.SaveToFileList(groups, filePath); // Oppdaterer filen
                }
            }
        }
        
        // Returnerer filstien for gruppedata
        private string GetFilePath()
        {
            return Path.Combine(_directoryPath, $"group.json");
        }
    }
}
