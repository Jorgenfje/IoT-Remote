using Backend.Domain;
using Backend.Ports;

namespace Backend.Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        // Injiserer repository for group
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // Legger til en enhet i en gruppe basert på gruppens ID
        public async Task AddDeviceToGroup(int groupId, Device device)
        {
            await _groupRepository.AddDeviceToGroup(groupId, device);
        }

        // Oppretter en ny gruppe
        public async Task AddGroup(Group group)
        {
            await _groupRepository.AddGroup(group);
        }

        // Sletter en gruppe basert på gruppens ID
        public async Task DeleteGroup(int groupId)
        {
            await _groupRepository.DeleteGroup(groupId);
        }

        // Henter alle grupper fra json-fil
        public async Task<List<Group>> GetAllGroups()
        {
            return await _groupRepository.GetAllGroups();
        }

        // Henter en spesifikk gruppe basert på ID
        public Task<Group> GetGroupById(int groupId)
        {
            return _groupRepository.GetGroupById(groupId);
        }

        // Henter en gruppe basert på gruppens navn
        public async Task<Group> GetGroupByName(string groupName)
        {
            var group = await _groupRepository.GetAllGroups();
            return group.FirstOrDefault(g => g.Name == groupName);
        }

        // Fjerner en enhet fra en gruppe basert på gruppens ID og enhetens ID
        public async Task RemoveDeviceFromGroup(int groupId, int deviceId)
        {
            await _groupRepository.RemoveDeviceFromGroup(groupId, deviceId);
        }

        // Oppdaterer state for alle enheter i en gruppe
        public async Task UpdateGroupDevice(int groupId, bool state)
        {
            // Henter alle grupper
            var groups = await _groupRepository.GetAllGroups();
            // Henter den spesifikke gruppen som skal oppdateres
            var group = await _groupRepository.GetGroupById(groupId);

            // Itererer gjennom alle enheter i gruppen
            foreach (var device in group.Devices)
            {
                // Oppdaterer tilstanden til enheten i den aktuelle gruppen
                await _groupRepository.UpdateGroup(groupId, device.Id, state);

                // Sjekker om samme enhet er i andre grupper og oppdaterer der også
                foreach (var otherGroup in groups)
                {
                    if (otherGroup.Devices.Any(d => d.Id == device.Id))
                    {
                        await _groupRepository.UpdateGroup(otherGroup.Id, device.Id, state);
                    }
                }
            }
        }
    }
}

