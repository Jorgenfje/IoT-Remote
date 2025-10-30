using Backend.Domain;
using Backend.Ports;
using Backend.Service;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Service
{
    public class LightServiceTests
    {
        private readonly Mock<ILightRepository> _lightRepositoryMock;
        private readonly LightService _lightService;

        public LightServiceTests()
        {
            _lightRepositoryMock = new Mock<ILightRepository>();
            _lightService = new LightService(_lightRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllLights_ShouldReturnAllLights()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1" },
                new Light { Id = 2, Name = "Light2" }
            };
            _lightRepositoryMock.Setup(repo => repo.GetAllDevices()).ReturnsAsync(mockLights);

            // Act
            var result = await _lightService.GetAllDevices();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Light1", result[0].Name);
            Assert.Equal("Light2", result[1].Name);
        }

        [Fact]
        public async Task GetLightById_ShouldReturnLight_WhenLightExists()
        {
            // Arrange
            var mockLight = new Light { Id = 1, Name = "Light1" };
            _lightRepositoryMock.Setup(repo => repo.GetDeviceById(1)).ReturnsAsync(mockLight);

            // Act
            var result = await _lightService.GetDeviceById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Light1", result.Name);
        }

        [Fact]
        public async Task GetLightById_ShouldReturnNull_WhenLightDoesNotExist()
        {
            // Arrange
            _lightRepositoryMock.Setup(repo => repo.GetDeviceById(1)).ReturnsAsync((Light)null);

            // Act
            var result = await _lightService.GetDeviceById(1);

            // Assert
            Assert.Null(result);
        }


        // Integration tests:
        [Fact]
        public async Task AddDevice_ShouldCallRepositoryAddDevice()
        {
            // Arrange
            var Light = new Light { Name = "Light" };

            // Act
            await _lightService.AddDevice(Light);

            // Assert
            _lightRepositoryMock.Verify(repo => repo.AddDevice(Light), Times.Once);
        }

        [Fact]
        public async Task DeleteDevice_ShouldCallRepositoryDeleteDevice()
        {
            // Arrange
            var lightId = 1;

            // Act
            await _lightService.DeleteDevice(lightId);

            // Assert
            _lightRepositoryMock.Verify(repo => repo.DeleteDevice(lightId), Times.Once);
        }

        [Fact]
        public async Task UpdateDevicePaired_ShouldCallRepositoryUpdateDevicePaired()
        {
            // Arrange
            var lightId = 1;
            var paired = true;

            // Act
            await _lightService.UpdateDevicePaired(lightId, paired);

            // Assert
            _lightRepositoryMock.Verify(repo => repo.UpdateDevicePaired(lightId, paired), Times.Once);
        }

        [Fact]
        public async Task UpdateDeviceState_ShouldCallRepositoryUpdateDeviceState()
        {
            // Arrange
            var lightId = 1;
            var state = true;

            // Act
            await _lightService.UpdateDeviceState(lightId, state);

            // Assert
            _lightRepositoryMock.Verify(repo => repo.UpdateDeviceState(lightId, state), Times.Once);
        }

        [Fact]
        public async Task UpdateLightFromGroup_ShouldUpdateAllDevicesInGroup()
        {
            // Arrange
            var device1 = new Device { Id = 1, Name = "Device1", State = false };
            var device2 = new Device { Id = 2, Name = "Device2", State = false };
            var devices = new List<Device> { device1, device2 };

            // Act
            await _lightService.UpdateLightFromGroup(devices, true);

            // Assert
            _lightRepositoryMock.Verify(repo => repo.UpdateDevicesFromGroup(device1.Id, true), Times.Once);
            _lightRepositoryMock.Verify(repo => repo.UpdateDevicesFromGroup(device2.Id, true), Times.Once);
        }
    }
}