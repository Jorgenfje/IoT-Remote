using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Ports;
using Backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class ApiKeyControllerTests
    {

        // Mocking dependencies:
        private readonly Mock<IApiKeyService> _apiKeyServiceMock;
        private readonly ApiKeyController _apiKeyController;
        
        public ApiKeyControllerTests()
        {
            _apiKeyServiceMock = new Mock<IApiKeyService>();
            _apiKeyController = new ApiKeyController(_apiKeyServiceMock.Object);
        }

        [Fact]
        public async Task RegisterNewRemote_ShouldReturnBadRequest_WhenRemoteIdAlreadyExists()
        {
            // Arrange
            string remoteId = "existingRemoteId";
            _apiKeyServiceMock.Setup(service => service.CheckIfExisting(remoteId)).ReturnsAsync(true);

            // Act
            var result = await _apiKeyController.RegisterNewRemote(remoteId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Allerede registrert", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterNewRemote_ShouldReturnOkWithApiKey_WhenRemoteIdDoesNotExist()
        {
            // Arrange
            string remoteId = "newRemoteId";
            var mockApiKey = new ApiKey
            {
                RemoteId = remoteId,
                Key = "mockGeneratedApiKey"
            };

            _apiKeyServiceMock.Setup(service => service.CheckIfExisting(remoteId)).ReturnsAsync(false);
            _apiKeyServiceMock.Setup(service => service.CreateApiKey(remoteId)).ReturnsAsync(mockApiKey);

            // Act
            var result = await _apiKeyController.RegisterNewRemote(remoteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockApiKey, okResult.Value);
        }
    }
}
