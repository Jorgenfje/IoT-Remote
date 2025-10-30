using Backend.Domain;
using Backend.Ports;
using Backend.Service;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Service
{
    public class GroupServiceTests
    {
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly GroupService _groupService;

        public GroupServiceTests()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _groupService = new GroupService(_groupRepositoryMock.Object);
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
            _groupRepositoryMock.Setup(repo => repo.GetAllGroups()).ReturnsAsync(mockGroups);

            // Act
            var result = await _groupService.GetAllGroups();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Group1", result[0].Name);
            Assert.Equal("Group2", result[1].Name);
        }

        [Fact]
        public async Task GetGroupById_ShouldReturnGroup_WhenGroupExists()
        {
            // Arrange
            var mockGroup = new Group { Id = 1, Name = "Group1" };
            _groupRepositoryMock.Setup(repo => repo.GetGroupById(1)).ReturnsAsync(mockGroup);

            // Act
            var result = await _groupService.GetGroupById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Group1", result.Name);
        }

        [Fact]
        public async Task GetGroupById_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            // Arrange
            _groupRepositoryMock.Setup(repo => repo.GetGroupById(1)).ReturnsAsync((Group)null);

            // Act
            var result = await _groupService.GetGroupById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGroupByName_ShouldReturnGroup_WhenGroupExists()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1" },
                new Group { Id = 2, Name = "Group2" }
            };
            _groupRepositoryMock.Setup(repo => repo.GetAllGroups()).ReturnsAsync(mockGroups);

            // Act
            var result = await _groupService.GetGroupByName("Group1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Group1", result.Name);
        }

        [Fact]
        public async Task GetGroupByName_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            // Arrange
            var mockGroups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1" },
                new Group { Id = 2, Name = "Group2" }
            };
            _groupRepositoryMock.Setup(repo => repo.GetAllGroups()).ReturnsAsync(mockGroups);

            // Act
            var result = await _groupService.GetGroupByName("Group3");

            // Assert
            Assert.Null(result);
        }


        // Integration tests:
        [Fact]
        public async Task AddGroup_ShouldCallRepositoryAddGroup()
        {
            // Arrange
            var newGroup = new Group { Name = "NewGroup" };

            // Act
            await _groupService.AddGroup(newGroup);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.AddGroup(newGroup), Times.Once);
        }

        [Fact]
        public async Task DeleteGroup_ShouldCallRepositoryDeleteGroup()
        {
            // Arrange
            var groupId = 1;

            // Act
            await _groupService.DeleteGroup(groupId);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.DeleteGroup(groupId), Times.Once);
        }

        [Fact]
        public async Task AddDeviceToGroup_ShouldCallRepositoryAddDeviceToGroup()
        {
            // Arrange
            var groupId = 1;
            var device = new Device { Id = 1, Name = "Device1" };

            // Act
            await _groupService.AddDeviceToGroup(groupId, device);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.AddDeviceToGroup(groupId, device), Times.Once);
        }

        [Fact]
        public async Task RemoveDeviceFromGroup_ShouldCallRepositoryRemoveDeviceFromGroup()
        {
            // Arrange
            var groupId = 1;
            var deviceId = 1;

            // Act
            await _groupService.RemoveDeviceFromGroup(groupId, deviceId);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.RemoveDeviceFromGroup(groupId, deviceId), Times.Once);
        }
    }
}