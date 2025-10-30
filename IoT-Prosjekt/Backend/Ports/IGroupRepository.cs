using Backend.Domain;

namespace Backend.Ports
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllGroups();
        Task<Group> GetGroupById(int id);
        Task AddGroup(Group group);
        Task AddDeviceToGroup(int groupId, Device device);
        Task RemoveDeviceFromGroup(int groupId, int deviceId);
        Task DeleteGroup(int groupId);
        Task UpdateGroup(int groupId, int deviceId, bool state);
    }
}
