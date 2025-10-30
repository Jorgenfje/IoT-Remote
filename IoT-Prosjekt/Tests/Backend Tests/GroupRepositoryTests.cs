using Backend.Domain;
using Backend.Ports;
using Backend.Repository;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Repository
{
    public class GroupRepositoryTests
    {
        private readonly Mock<IJsonFileHandler<Group>> _jsonFileHandlerMock;
        private readonly GroupRepository _groupRepository;

        public GroupRepositoryTests()
        {
            _jsonFileHandlerMock = new Mock<IJsonFileHandler<Group>>();
            _groupRepository = new GroupRepository(_jsonFileHandlerMock.Object);
        }

        [Fact]
        public async Task AddDeviceToGroup_ShouldAddDeviceToGroup_WhenGroupExists()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1", Devices = new List<Device>() }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockGroups);

            var newDevice = new Device { Id = 1, Name = "Device1" };

            // Act
            await _groupRepository.AddDeviceToGroup(1, newDevice);

            // Assert
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Group>>(g => g.First().Devices.Count == 1), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddGroup_ShouldAddGroupWithIncrementedId()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockGroups);

            var newGroup = new Group { Name = "Group2" };

            // Act
            await _groupRepository.AddGroup(newGroup);

            // Assert
            Assert.Equal(2, newGroup.Id); // Checking that the new group has an incremented ID
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Group>>(g => g.Count == 2), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteGroup_ShouldRemoveGroup_WhenGroupExists()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1" },
                new Group { Id = 2, Name = "Group2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockGroups);

            // Act
            await _groupRepository.DeleteGroup(1);

            // Assert
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Group>>(g => g.Count == 1 && g.First().Id == 2), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllGroups_ShouldReturnAllGroups()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1" },
                new Group { Id = 2, Name = "Group2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockGroups);

            // Act
            var result = await _groupRepository.GetAllGroups();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Group1", result[0].Name);
            Assert.Equal("Group2", result[1].Name);
        }

        [Fact]
        public async Task GetGroupById_ShouldReturnGroup_WhenGroupExists()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1" },
                new Group { Id = 2, Name = "Group2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockGroups);

            // Act
            var result = await _groupRepository.GetGroupById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Group1", result.Name);
        }

        [Fact]
        public async Task GetGroupById_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            // Arrange
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(new List<Group>());

            // Act
            var result = await _groupRepository.GetGroupById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveDeviceFromGroup_ShouldRemoveDeviceFromGroup_WhenDeviceExists()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1", Devices = new List<Device> { new Device { Id = 1, Name = "Device1" } } }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockGroups);

            // Act
            await _groupRepository.RemoveDeviceFromGroup(1, 1);

            // Assert
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Group>>(g => g.First().Devices.Count == 0), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateGroup_ShouldUpdateDeviceStateInAllGroups()
        {
            // Arrange
            var device1 = new Device { Id = 1, Name = "Device1", State = false };
            var device2 = new Device { Id = 2, Name = "Device2", State = false };

            var group1 = new Group
            {
                Id = 1,
                Name = "Group1",
                Devices = new List<Device> { device1, device2 }
            };

            var group2 = new Group
            {
                Id = 2,
                Name = "Group2",
                Devices = new List<Device> { device1 } // Device1 is shared between Group1 and Group2
            };

            var allGroups = new List<Group> { group1, group2 };

            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(allGroups);

            // Act
            await _groupRepository.UpdateGroup(group1.Id, device1.Id, true);

            // Assert
            // Verify that device1's state has been updated in both groups
            Assert.True(group1.Devices.First(d => d.Id == device1.Id).State);
            Assert.True(group2.Devices.First(d => d.Id == device1.Id).State);

            // Verify that device2's state has not changed
            Assert.False(group1.Devices.First(d => d.Id == device2.Id).State);
        }
    }
}