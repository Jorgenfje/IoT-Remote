using Backend.Domain;
using Backend.Dto;
using Backend.Ports;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Backend.Controllers
{
    // Setter opp url til denne Controller
    [Route("/api/v1/[controller]")]
    [ApiController]
public class GroupController : Controller
{
    private readonly IGroupService _groupService;
    private readonly ILightService _lightService;

    // Injisere avhengigheter for group- og Light-service
    public GroupController(IGroupService groupService, ILightService lightService)
    {
        _groupService = groupService;
        _lightService = lightService;
    }

    // Endepunkt for å opprette en ny gruppe
    [HttpPost("createGroup")]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto request)
    {
        // Henter en enhet basert på ID
        var device = await _lightService.GetDeviceById(request.Id);

        if (device != null)
        {
            // Henter alle grupper og sjekker om gruppenavnet allerede eksisterer
            var checkGroups = await _groupService.GetAllGroups();
            var checkGroup = checkGroups.FirstOrDefault(g => g.Name == request.GroupName);

            if (checkGroup != null)
            {
                // Hvis gruppen finnes, legg til enheten i gruppen
                await _groupService.AddDeviceToGroup(checkGroup.Id, device);
            }
            else
            {
                // Hvis gruppen ikke finnes, opprett en ny gruppe med enheten
                var newGroup = new Group()
                {
                    Name = request.GroupName,
                    Devices = new List<Device> { device }
                };

                await _groupService.AddGroup(newGroup);
            }
        }
        // Returnerer enheten som ble lagt til
        return Ok(device); 
    }

    // Endepunkt for å hente alle grupper
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllGroups()
    {
        // Returnerer alle grupper som en OK-respons
        return Ok(await _groupService.GetAllGroups());
    }

    // Endepunkt for å slette en gruppe
    [HttpPost("deleteGroup/{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        // Henter gruppen basert på ID
        var group = await _groupService.GetGroupById(id);

        if (group != null)
        {
            // Hvis gruppen finnes, slett den
            await _groupService.DeleteGroup(id);
            return Ok("Gruppen har blitt slettet");
        }
        // Returnerer feil hvis gruppen ikke finnes
        return BadRequest(); 
    }

    // Endepunkt for å slette en enhet fra en gruppe
    [HttpPost("deleteDeviceFromGroup")]
    public async Task<IActionResult> DeleteDeviceFromGroup([FromBody] DeleteDeviceFromGroupDto request)
    {
        // Henter gruppen basert på ID
        var group = await _groupService.GetGroupById(request.GroupId);
        if (group != null)
        {
            // Sjekker om enheten finnes i gruppen
            if (!group.Devices.Any(d => d.Id == request.DeviceId))
            {
                return BadRequest(); // Returnerer feil hvis enheten ikke finnes
            }
            // Fjerner enheten fra gruppen
            await _groupService.RemoveDeviceFromGroup(group.Id, request.DeviceId);
            return Ok("Enheten har blitt slettet");
        }
        return BadRequest(); // Returnerer feil hvis gruppen ikke finnes
    }

    // Endepunkt for å oppdatere statusen til enheter i en gruppe
    [HttpPost("updateDevices")]
    public async Task<IActionResult> UpdateDevices([FromBody] ChangeDevicesFromGroupDto request)
    {
        // Henter gruppen basert på ID
        var groups = await _groupService.GetAllGroups();
        var group = groups.FirstOrDefault(g => g.Id == request.Id);
        // Hvis gruppen eksisterer
        if (group != null)
        {
            // Oppdaterer både group.json og light.json filene
            _lightService.UpdateLightFromGroup(group.Devices, request.State);
            _groupService.UpdateGroupDevice(group.Id, request.State);
            return Ok();
        }
        return BadRequest(); // Returnerer feil hvis gruppen ikke finnes
    }
}
}
