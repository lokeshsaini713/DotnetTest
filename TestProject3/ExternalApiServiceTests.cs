using System.Net;
using System.Text;

namespace TestProject3
{

    public class ExternalApiServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockFactory;
        private readonly ExternalApiService _service;

        public ExternalApiServiceTests()
        {
            _mockFactory = new Mock<IHttpClientFactory>();
            _service = new ExternalApiService(_mockFactory.Object);
        }

        [Fact]
        public async Task GetDataAsync_ReturnsDataSet_WhenResponseIsSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Setup(handler => handler.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Table\":[{\"Column\":\"Value\"}]}", Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var result = await _service.GetDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Value", result.Tables[0].Rows[0]["Column"]);
        }
    }


}