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
    public class GroupServiceTest
    {
        // This test is basically the same as the one for the DeviceService.
        [Fact]
        public async Task GetGroupsAsync_ReturnsGroupsList()
        {
            // Arrange

            // Mocking http client:
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()

            // Configuring SendAsync method to return an http 200 response and a list of devices:
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new List<Group>
                    {
                        new Group { Name = "Group1" },
                        new Group { Name = "Group2" }
                    })
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var groupService = new GroupService(httpClient);

            // Act
            var devices = await groupService.GetGroupsAsync();

            // Assert
            Assert.NotNull(devices);
            Assert.Equal(2, devices.Count);
            Assert.Equal("Group1", devices[0].Name);
            Assert.Equal("Group2", devices[1].Name);
        }
    }
}