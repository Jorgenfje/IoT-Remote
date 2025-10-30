using Backend.Domain;
using Backend.Ports;
using Backend.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace Backend.Tests.Repository
{
    public class LightRepositoryTests
    {
        private readonly Mock<IJsonFileHandler<Light>> _jsonFileHandlerMock;
        private readonly LightRepository _lightRepository;

        public LightRepositoryTests()
        {
            // Mocking JsonFileHandler for Light objects
            _jsonFileHandlerMock = new Mock<IJsonFileHandler<Light>>();
            _lightRepository = new LightRepository(_jsonFileHandlerMock.Object);
        }

        [Fact]
        public async Task GetAllDevices_ShouldReturnLights()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1" },
                new Light { Id = 2, Name = "Light2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            var result = await _lightRepository.GetAllDevices();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Light1", result[0].Name);
            Assert.Equal("Light2", result[1].Name);
        }

        [Fact]
        public async Task GetDeviceById_ShouldReturnLight_WhenLightExists()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1" },
                new Light { Id = 2, Name = "Light2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            var result = await _lightRepository.GetDeviceById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Light1", result.Name);
        }

        [Fact]
        public async Task GetDeviceById_ShouldReturnNull_WhenLightDoesNotExist()
        {
            // Arrange
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(new List<Light>());

            // Act
            var result = await _lightRepository.GetDeviceById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddDevice_ShouldAddLightWithIncrementedId()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            var newLight = new Light { Name = "Light2" };

            // Act
            await _lightRepository.AddDevice(newLight);

            // Assert
            Assert.Equal(2, newLight.Id);
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Light>>(l => l.Count == 2), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDevice_ShouldRemoveLight_WhenLightExists()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1" },
                new Light { Id = 2, Name = "Light2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            await _lightRepository.DeleteDevice(1);

            // Assert
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Light>>(l => l.Count == 1 && l.All(light => light.Id == 2)), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDevice_ShouldNotModifyList_WhenLightDoesNotExist()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1" },
                new Light { Id = 2, Name = "Light2" }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            await _lightRepository.DeleteDevice(3);

            // Assert
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.IsAny<List<Light>>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDevicePaired_ShouldUpdatePairedState_WhenLightExists()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1", Paired = false },
                new Light { Id = 2, Name = "Light2", Paired = false }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            await _lightRepository.UpdateDevicePaired(1, true);

            // Assert
            var updatedLight = mockLights.First(d => d.Id == 1);
            Assert.True(updatedLight.Paired);
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Light>>(l => l.First(d => d.Id == 1).Paired == true), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDeviceState_ShouldUpdateState_WhenLightExists()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1", State = false },
                new Light { Id = 2, Name = "Light2", State = false }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            await _lightRepository.UpdateDeviceState(1, true);

            // Assert
            var updatedLight = mockLights.First(d => d.Id == 1);
            Assert.True(updatedLight.State);
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Light>>(l => l.First(d => d.Id == 1).State == true), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDevicesFromGroup_ShouldUpdateState_WhenLightExists()
        {
            // Arrange
            var mockLights = new List<Light>
            {
                new Light { Id = 1, Name = "Light1", State = false },
                new Light { Id = 2, Name = "Light2", State = false }
            };
            _jsonFileHandlerMock.Setup(handler => handler.ReadFromFileList(It.IsAny<string>())).ReturnsAsync(mockLights);

            // Act
            await _lightRepository.UpdateDevicesFromGroup(1, true);

            // Assert
            var updatedLight = mockLights.First(d => d.Id == 1);
            Assert.True(updatedLight.State);
            _jsonFileHandlerMock.Verify(handler => handler.SaveToFileList(It.Is<List<Light>>(l => l.First(d => d.Id == 1).State == true), It.IsAny<string>()), Times.Once);
        }
    }
}
