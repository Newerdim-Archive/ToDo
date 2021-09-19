using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using ToDo.API;
using ToDo.API.Const;
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

        [Fact]
        public async Task CreateAsync_ModelIsInvalid_ReturnsBadRequestWithMessageAndError()
        {
            // Arrange
            var model = new CreateToDoModel();

            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.CreateToDo, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be("One or more validation errors occurred");
            content.Errors.Should().ContainSingle(x =>
                x.Property == "Title" &&
                x.Messages.Contains("'Title' is required")
            );
        }

        #endregion

        #region GetAllUserTodosAsync

        [Fact]
        public async Task GetAllUserTodosAsync_IsAuthenticated_ReturnsOkWithMessage()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetAllUserToDos);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<List<API.Dto.ToDo>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Got all user to-do's successfully");
        }

        [Fact]
        public async Task GetAllUserTodosAsync_IsAuthenticated_ReturnsToDos()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetAllUserToDos);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<List<API.Dto.ToDo>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetAllUserTodosAsync_IsNotAuthenticated_ReturnsUnauthorizedWithMessage()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetAllUserToDos);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("You do not have permission. Please, log in first");
        }

        #endregion

        #region GetByIdFromUserAsync

        [Fact]
        public async Task GetByIdFromUserAsync_Exists_ReturnsOkWithMessage()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetByIdFromUser + "1");

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();
            content!.Message.Should().Be("Got to-do successfully");
        }

        [Fact]
        public async Task GetByIdFromUserAsync_Exists_ReturnsCorrectToDo()
        {
            // Arrange
            const int id = 1;

            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetByIdFromUser + id);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNull();
            content.Data.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetByIdFromUserAsync_NotExist_ReturnsNotFoundWithMessage()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetByIdFromUser + "99");

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            content.Should().NotBeNull();
            content!.Message.Should().Be("Resource not found");
        }

        [Fact]
        public async Task GetByIdFromUserAsync_NotAuthenticated_ReturnsUnauthorizedWithMessage()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetByIdFromUser + "1");

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();
            content!.Message.Should().Be(ResponseMessage.Unauthorized);
        }

        #endregion
    }
}