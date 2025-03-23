using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Threading;

namespace UserManagement.API.Test.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient CreateMockHttpClient(HttpStatusCode statusCode = HttpStatusCode.OK, string content = "{}")
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });

            return new HttpClient(mockHttpMessageHandler.Object);
        }

        public static void SetupHttpClientFactory(Mock<IHttpClientFactory> mockHttpClientFactory, string clientName, HttpStatusCode statusCode = HttpStatusCode.OK, string content = "{}")
        {
            var httpClient = CreateMockHttpClient(statusCode, content);
            mockHttpClientFactory.Setup(x => x.CreateClient(clientName))
                .Returns(httpClient);
        }

        public static void SetupConfiguration(Mock<IConfiguration> mockConfiguration, string domain = "messaging-service", string port = "3002")
        {
            var mockDomainSection = new Mock<IConfigurationSection>();
            mockDomainSection.Setup(x => x.Value).Returns(domain);
            mockConfiguration.Setup(x => x.GetSection("MessagingService:Domain"))
                .Returns(mockDomainSection.Object);

            var mockPortSection = new Mock<IConfigurationSection>();
            mockPortSection.Setup(x => x.Value).Returns(port);
            mockConfiguration.Setup(x => x.GetSection("MessagingService:Port"))
                .Returns(mockPortSection.Object);
        }
    }
} 