using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ToDo.API;
using ToDo.API.Const;
using ToDo.API.Dto;
using ToDo.API.Responses;
using ToDo.IntegrationTests.Helpers;
using Xunit;

namespace ToDo.IntegrationTests.Controllers
{
    public class ProfilesControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public ProfilesControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region GetCurrentAsync

        [Fact]
        public async Task GetCurrentAsync_IsAuthenticated_ReturnsOkWithMessage()
        {
            // Arrange
            _httpClient.AddAuthenticationToken(AccessToken.Valid);

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetCurrentProfile);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<Profile>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.ResourceGottenSuccessfully);
        }

        [Fact]
        public async Task GetCurrentAsync_IsAuthenticated_ReturnsCorrectProfile()
        {
            // Arrange
            _httpClient.AddAuthenticationToken(AccessToken.Valid);

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetCurrentProfile);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<Profile>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNull();

            content.Data.Username.Should().Be("User1");
            content.Data.Email.Should().Be("User1@mail.com");
            content.Data.ProfilePictureUrl.Should().Be("www.example.com/picure/1");
        }

        [Fact]
        public async Task GetCurrentAsync_UserNotExist_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            _httpClient.AddAuthenticationToken(AccessToken.WithNotExistingUser);

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetCurrentProfile);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("User not exist");
        }

        #endregion
    }
}