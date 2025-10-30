using Backend.Domain;

namespace Backend.Ports
{
    public interface IGroupService
    {
        Task<List<Group>> GetAllGroups();
        Task<Group> GetGroupById(int groupId);
        Task<Group> GetGroupByName(string groupName);
        Task AddGroup(Group group);
        Task AddDeviceToGroup(int groupId, Device device);
        Task RemoveDeviceFromGroup(int groupId, int deviceId);
        Task DeleteGroup(int groupId);
        Task UpdateGroupDevice(int groupId, bool state);
    }
}
