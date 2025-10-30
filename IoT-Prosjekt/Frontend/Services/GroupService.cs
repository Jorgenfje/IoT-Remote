using Frontend.Dto;
using Frontend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Frontend.Services
{
    public class GroupService
    {
        // HttpClient brukes til å kommunisere med backend-API
        private readonly HttpClient _httpClient;
        private readonly int _port = 5048; // Port til backenden

        public GroupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Henter en liste over alle grupper fra backend
        public async Task<List<Group>> GetGroupsAsync()
        {
            var response = await _httpClient.GetAsync($"http://localhost:{_port}/api/v1/group/getAll"); // Sender GET-forespørsel til API-et
            response.EnsureSuccessStatusCode();
            var groups = await response.Content.ReadFromJsonAsync<List<Group>>(); // Lagrer det i groups
            return groups; // Returnerer listen over grupper
        }

        // Oppretter en ny gruppe og legger til en enhet i gruppen
        public async Task CreateGroupWithDevice(string groupName, int deviceId)
        {
            // Oppretter et Dto-objekt
            var request = new CreateGroupDto()
            {
                GroupName = groupName,
                Id = deviceId
            };
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:{_port}/api/v1/Group/createGroup", request); // Sender POST-forespørsel for å opprette gruppen
            response.EnsureSuccessStatusCode();
        }

        // Fjerner en enhet fra en gruppe
        public async Task RemoveDeviceFromGroup(int groupId, int deviceId)
        {
            // Oppretter et Dto-objekt
            var request = new RemoveGroupFromGroupDto()
            {
                GroupId = groupId, 
                DeviceId = deviceId 
            };
            var response =
                await _httpClient.PostAsJsonAsync($"http://localhost:{_port}/api/v1/Group/deleteDeviceFromGroup",
                    request); // Sender POST-forespørsel for å fjerne enheten fra gruppen
            response.EnsureSuccessStatusCode();
        }

        // Sletter en gruppe
        public async Task RemoveGroup(int groupId)
        {
            var response = await _httpClient.PostAsync($"http://localhost:{_port}/api/v1/Group/deleteGroup/{groupId}", null); // Sender POST-forespørsel for å slette gruppen
            response.EnsureSuccessStatusCode();
        }
    }
}

