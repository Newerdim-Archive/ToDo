using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using ToDo.API;
using ToDo.API.Models;
using ToDo.API.Responses;
using ToDo.IntegrationTests.Helpers;
using Xunit;

namespace ToDo.IntegrationTests.Controllers
{
    public class ToDoControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public ToDoControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region CreateAsync

        [Theory, AutoData]
        public async Task CreateAsync_ModelIsValid_ReturnsOkWithMessage(CreateToDoModel model)
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.CreateToDo, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Created To-Do successfully");
        }

        [Fact]
        public async Task CreateAsync_UserIsNotAuthenticated_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            var model = new CreateToDoModel();

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.CreateToDo, model);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("You do not have permission. Please, log in first");
        }
        
        [Theory, AutoData]
        public async Task CreateAsync_ModelIsValid_ReturnsCreatedToDo(CreateToDoModel model)
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.CreateToDo, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNull();

            content.Data.Id.Should().BeGreaterThan(0);
            content.Data.Title.Should().Be(model.Title);
            content.Data.Description.Should().Be(model.Description);
            content.Data.Deadline.Should().Be(model.Deadline);
            content.Data.Completed.Should().BeFalse();
        }

        #endregion
    }
}