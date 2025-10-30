using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Domain;
using Backend.Dto;
using Backend.Ports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class GroupControllerTests
    {
        private readonly Mock<IGroupService> _groupServiceMock;
        private readonly Mock<ILightService> _lightServiceMock;
        private readonly GroupController _groupController;

        public GroupControllerTests()
        {
            _groupServiceMock = new Mock<IGroupService>();
            _lightServiceMock = new Mock<ILightService>();
            _groupController = new GroupController(_groupServiceMock.Object, _lightServiceMock.Object);
        }

        [Fact]
        public async Task CreateGroup_ShouldAddDeviceToExistingGroup_WhenGroupExists()
        {
            // Arrange
            var request = new CreateGroupDto { Id = 1, GroupName = "TestGroup" };
            var device = new Light { Id = request.Id, Name = "Device1" };
            var existingGroup = new Group { Id = 1, Name = request.GroupName, Devices = new List<Device>() };

            _lightServiceMock.Setup(service => service.GetDeviceById(request.Id)).ReturnsAsync(device);
            _groupServiceMock.Setup(service => service.GetAllGroups()).ReturnsAsync(new List<Group> { existingGroup });

            // Act
            var result = await _groupController.CreateGroup(request);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(device, okResult.Value);
        }

        [Fact]
        public async Task CreateGroup_ShouldCreateNewGroup_WhenGroupDoesNotExist()
        {
            // Arrange
            var request = new CreateGroupDto { Id = 1, GroupName = "NewGroup" };
            var device = new Light { Id = request.Id, Name = "Device1" };

            _lightServiceMock.Setup(service => service.GetDeviceById(request.Id)).ReturnsAsync(device);
            _groupServiceMock.Setup(service => service.GetAllGroups()).ReturnsAsync(new List<Group>());

            // Act
            var result = await _groupController.CreateGroup(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(device, okResult.Value);
        }

        [Fact]
        public async Task GetAllGroups_ShouldReturnOkWithAllGroups()
        {
            // Arrange
            var groups = new List<Group> { new Group { Id = 1, Name = "Group1" } };
            _groupServiceMock.Setup(service => service.GetAllGroups()).ReturnsAsync(groups);

            // Act
            var result = await _groupController.GetAllGroups();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(groups, okResult.Value);
        }

        // Can't test the state of the devices in the following tests because the methods are only returning Ok with a string
        [Fact]
        public async Task DeleteGroup_ShouldReturnOk_WhenGroupExists()
        {
            // Arrange
            var groupId = 1;
            var group = new Group { Id = groupId, Name = "TestGroup" };

            _groupServiceMock.Setup(service => service.GetGroupById(groupId)).ReturnsAsync(group);
            _groupServiceMock.Setup(service => service.DeleteGroup(groupId)).Returns(Task.CompletedTask);

            // Act
            var result = await _groupController.DeleteGroup(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Gruppen har blitt slettet", okResult.Value);
        }

        [Fact]
        public async Task DeleteGroup_ShouldReturnBadRequest_WhenGroupDoesNotExist()
        {
            // Arrange
            var groupId = 1;
            _groupServiceMock.Setup(service => service.GetGroupById(groupId)).ReturnsAsync((Group)null);

            // Act
            var result = await _groupController.DeleteGroup(groupId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateDevices_ShouldReturnOk_WhenGroupExists()
        {
            // Arrange
            var groupId = 1;
            var group = new Group
            {
                Id = groupId,
                Name = "TestGroup",
                Devices = new List<Device> { new Light { Id = 1, Name = "Device1" } }
            };
            var request = new ChangeDevicesFromGroupDto { Id = groupId, State = true };

            _groupServiceMock.Setup(service => service.GetAllGroups()).ReturnsAsync(new List<Group> { group });

            // Act
            var result = await _groupController.UpdateDevices(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateDevices_ShouldReturnBadRequest_WhenGroupDoesNotExist()
        {
            // Arrange
            var request = new ChangeDevicesFromGroupDto { Id = 1, State = true };
            _groupServiceMock.Setup(service => service.GetAllGroups()).ReturnsAsync(new List<Group>());

            // Act
            var result = await _groupController.UpdateDevices(request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
