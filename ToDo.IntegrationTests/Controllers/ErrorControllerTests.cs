using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ToDo.API;
using ToDo.API.Const;
using ToDo.API.Responses;
using ToDo.IntegrationTests.Helpers;
using Xunit;

namespace ToDo.IntegrationTests.Controllers
{
    public class ErrorControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public ErrorControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region Error

        [Fact]
        public async Task Error_ReturnsInternalServerErrorWithMessage()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync("api/error");

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            content.Should().NotBeNull();
            content!.Message.Should().Be(ResponseMessage.UnexpectedError);
        }

        #endregion
    }
}