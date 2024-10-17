using API;
using API.Controllers;
using Moq;

namespace TestProject3
{
    public partial class UnitTest1
    {
        public class ExternalDataControllerTests
        {

            private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
            private readonly ExternalApiService _service;

            public ExternalDataControllerTests()
            {
                _mockHttpClientFactory = new Mock<IHttpClientFactory>();

                // Initialize the ExternalApiService with the mocked HttpClientFactory
                _service = new ExternalApiService(_mockHttpClientFactory.Object);
            }


            [Fact]
            public async Task FetchDataFromApiAsync_ReturnsDataSet_WhenApiReturnsValidResponse()
            {

                //arrange
                var clientHandlerStub = new DelegatingHandlerStub();
                var client = new HttpClient(clientHandlerStub);

                _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
                // Act
                ExternalDataController home = new ExternalDataController(_service);
                var result1 = await home.GetDataByAPI();

                // Assert
                Assert.NotNull(result1);
                Assert.Equal("true", result1 != null ? "true" : "false");
            }

            [Fact]
            public async Task FetchDataFromApiAsync_ThrowsException_WhenApiReturnsError()
            {
                // Act & Assert
                await Assert.ThrowsAsync<HttpRequestException>(() => _service.FetchDataFromApiAsync("https://fakeapi.org/posts"));
            }

        }
    }
    
}