using BusinessLogicLayer.Services;
using Moq.Protected;
using Moq;
using System.Net;

namespace BusinessLogicLayer.Tests
{
    public class TranslateTol33tsp34kServiceTests
    {
        [Fact]
        public async Task TranslateToLeetSpeakAsync_ValidJson_ReturnsTranslatedText()
        {
            // Arrange
            var original = "hello";
            var translated = "h3ll0";

            var fakeJson =
                @"{""success"":{""total"":1},""contents"":{" +
                  @"""translated"":""" + translated + @"""," +
                  @"""text"":""" + original + @"""," +
                  @"""translation"":""leetspeak""}}";

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(req =>
                       req.Method == HttpMethod.Get
                       && req.RequestUri!.ToString().Contains("leetspeak.json")
                   ),
                   ItExpr.IsAny<CancellationToken>()
                )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(fakeJson),
               })
               .Verifiable();

            // HttpClient 
            var client = new HttpClient(handlerMock.Object);

            var service = new TranslateTol33tsp34kService(client);

            // Act
            var result = await service.TranslateToLeetSpeakAsync(original);

            // Assert
            Assert.Equal(translated, result);

   
            handlerMock.Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}