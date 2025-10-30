using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Frontend.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;
        private readonly DeviceService _deviceService;

        public GroupController(GroupService groupService, DeviceService deviceService)
        {
            _groupService = groupService;
            _deviceService = deviceService;
        }

        // Viser en oversikt over alle gruppene
        public async Task<IActionResult> Index()
        {
            List<Group> groups = await _groupService.GetGroupsAsync(); // Henter listen med gruppene
            return View(groups); // Sender gruppelisten til "Index"-visningen
        }

        // Viser skjema for å opprette en ny gruppe
        public async Task<IActionResult> Create()
        {
            List<Device> devices = await _deviceService.GetDevicesAsync(); // Henter listen over enheter
            var groups = await _groupService.GetGroupsAsync(); // Henter listen med grupper
            ViewBag.Devices = devices; // Legger enhetene i ViewBag for bruk i visningen
            return View(groups); // Sender gruppelisten til "Create"-visningen
        }

        // Viser en redigeringsside for en spesifikk gruppe
        public async Task<IActionResult> Edit(int groupId)
        {
            var groups = await _groupService.GetGroupsAsync(); // Henter listen med gruppene
            var group = groups.FirstOrDefault(g => g.Id == groupId); // Finner gruppen med spesifisert ID
            return View(group); // Sender gruppen til "Edit"-visningen
        }

        // Oppretter en ny gruppe og legger til en enhet
        [HttpPost]
        public async Task<IActionResult> CreateGroup(string groupName, int deviceId)
        {
            await _groupService.CreateGroupWithDevice(groupName, deviceId); // Oppretter gruppen med en enhet
            return RedirectToAction("Create"); // Går tilbake til "Create"-siden
        }

        // Fjerner en enhet fra en gruppe
        [HttpPost]
        public async Task<IActionResult> RemoveDeviceFromGroup(int groupId, int deviceId)
        {
            await _groupService.RemoveDeviceFromGroup(groupId, deviceId); // Fjerner enheten fra gruppen
            return RedirectToAction("Index"); // Går tilbake til "Index"-siden
        }

        // Sletter en gruppe basert på ID
        [HttpPost]
        public async Task<IActionResult> RemoveGroup(int groupId)
        {
            await _groupService.RemoveGroup(groupId); // Sletter gruppen
            return RedirectToAction("Index"); // Går tilbake til "Index"-siden
        }
    }
}

