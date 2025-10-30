using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Domain;
using Backend.Ports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;


namespace Backend.Tests.Controllers
{
    public class DeviceControllerTests
    {
        private readonly Mock<ILightService> _lightServiceMock;
        private readonly DeviceController _deviceController;

        public DeviceControllerTests()
        {
            _lightServiceMock = new Mock<ILightService>();
            _deviceController = new DeviceController(_lightServiceMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_AndAddedLight()
        {
            // Arrange
            var light = new Light
            {
                Id = 1,
                Name = "Test Light",
            };
            
            _lightServiceMock.Setup(service => service.AddDevice(light)).Returns(Task.CompletedTask);

            // Act
            var result = await _deviceController.Add(light);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(light, okResult.Value);
        }
        
        [Fact]
        public async Task GetAllDevices_ShouldReturnOk_AndAllDevices()
        {
            // Arrange
            var lights = new List<Light>
            {
                new Light
                {
                    Id = 1,
                    Name = "Light1",
                },
                new Light
                {
                    Id = 2,
                    Name = "Light2",
                }
            };
            _lightServiceMock.Setup(service => service.GetAllDevices()).ReturnsAsync(lights);

            // Act
            var result = await _deviceController.GetAllDevices();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lights, okResult.Value);
        }

        [Fact]
        public async Task DeleteById_ShouldReturnOk_AndDeletedDevice()
        {
            // Arrange
            var lights = new List<Light>
            {
                new Light
                {
                    Id = 1,
                    Name = "Light1",
                },
                new Light
                {
                    Id = 2,
                    Name = "Light2",
                }
            };
            var id = 1;
            var light = lights[0];
            
            _lightServiceMock.Setup(service => service.GetAllDevices()).ReturnsAsync(lights);
            _lightServiceMock.Setup(service => service.DeleteDevice(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _deviceController.DeleteById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(light, okResult.Value);
        }


        // Can't test paired state update because the method returns OkObjectResult with a string rather than the paired value
        [Fact]
        public async Task UpdateDevicePaired_ShouldReturnOk_WhenDeviceExists()
        {
            // Arrange
            var id = 1;
            var paired = true;
            var light = new Light
            {
                Id = id,
                Name = "Light1",
            };
            _lightServiceMock.Setup(service => service.GetDeviceById(id)).ReturnsAsync(light);
            _lightServiceMock.Setup(service => service.UpdateDevicePaired(id, paired)).Returns(Task.CompletedTask);

            // Act
            var result = await _deviceController.UpdateDevicePaired(id, paired);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Alt gikk bra", okResult.Value);
        }

        [Fact]
        public async Task UpdateDevicePaired_ShouldReturnBadRequest_WhenDeviceDoesNotExist()
        {
            // Arrange
            var id = 1;
            var paired = true;
            _lightServiceMock.Setup(service => service.GetDeviceById(id)).ReturnsAsync((Light)null);

            // Act
            var result = await _deviceController.UpdateDevicePaired(id, paired);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Fant ikke enheten", badRequestResult.Value);
        }

        // Tests for update state are the same as for update paired, same issue with state.
        [Fact]
        public async Task UpdateDeviceState_ShouldReturnOk_WhenDeviceExists()
        {
            // Arrange
            var id = 1;
            var state = true;
            var light = new Light
            {
                Id = id,
                Name = "Light1",
            };
            _lightServiceMock.Setup(service => service.GetDeviceById(id)).ReturnsAsync(light);
            _lightServiceMock.Setup(service => service.UpdateDeviceState(id, state)).Returns(Task.CompletedTask);

            // Act
            var result = await _deviceController.UpdateDeviceState(id, state);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateDeviceState_ShouldReturnBadRequest_WhenDeviceDoesNotExist()
        {
            // Arrange
            var id = 1;
            var state = true;
            _lightServiceMock.Setup(service => service.GetDeviceById(id)).ReturnsAsync((Light)null);

            // Act
            var result = await _deviceController.UpdateDeviceState(id, state);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }
    }
}
