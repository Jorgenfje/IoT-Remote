using System.Threading.Tasks;
using Backend.Domain;
using Backend.Ports;
using Backend.Service;
using Moq;
using Xunit;

namespace Backend.Tests.Service
{
    public class ApiKeyServiceTests
    {
        private readonly Mock<IApiKeyRepository> _apiKeyRepositoryMock;
        private readonly ApiKeyService _apiKeyService;

        public ApiKeyServiceTests()
        {
            _apiKeyRepositoryMock = new Mock<IApiKeyRepository>();
            _apiKeyService = new ApiKeyService(_apiKeyRepositoryMock.Object);
        }

        [Fact]
        public async Task CheckIfExisting_ShouldReturnTrue_WhenApiKeyExists()
        {
            // Arrange
            var remoteId = "existingRemoteId";
            var mockApiKey = new ApiKey { RemoteId = remoteId, Key = "testApiKey" };
            _apiKeyRepositoryMock.Setup(repo => repo.GetApiKeyByRemoteId(remoteId)).Returns(Task.FromResult(mockApiKey));

            // Act
            var result = await _apiKeyService.CheckIfExisting(remoteId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfExisting_ShouldReturnFalse_WhenApiKeyDoesNotExist()
        {
            // Arrange
            var remoteId = "nonexistentRemoteId";
            _apiKeyRepositoryMock.Setup(repo => repo.GetApiKeyByRemoteId(remoteId)).Returns(Task.FromResult<ApiKey>(null));

            // Act
            var result = await _apiKeyService.CheckIfExisting(remoteId);

            // Assert
            Assert.False(result);
        }
    }
}
