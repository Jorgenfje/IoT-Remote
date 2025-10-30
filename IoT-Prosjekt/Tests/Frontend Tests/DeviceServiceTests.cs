using Frontend.Models;
using Frontend.Services;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Frontend.Tests.Service
{
    public class DeviceServiceTest
    {
        [Fact]
        public async Task GetDevicesAsync_ReturnsDeviceList()
        {
            // Arrange

            // Mocking http client:
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()

            // Configuring SendAsync method to return an http 200 response and a list of devices in JSON format:
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new List<Device>
                    {
                        new Device { Id = 1, Name = "Device1" },
                        new Device { Id = 2, Name = "Device2" }
                    })
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var deviceService = new DeviceService(httpClient);

            // Act
            var devices = await deviceService.GetDevicesAsync();

            // Assert
            Assert.NotNull(devices);
            Assert.Equal(2, devices.Count);
            Assert.Equal("Device1", devices[0].Name);
            Assert.Equal("Device2", devices[1].Name);
        }
    }
}