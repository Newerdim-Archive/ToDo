using System;
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
    public class ToDosControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public ToDosControllerTests(CustomWebApplicationFactory<Startup> factory)
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

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_IsAuthenticated_ReturnsOkWithMessage()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetAllToDos);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<List<API.Dto.ToDo>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Got all user to-do's successfully");
        }

        [Fact]
        public async Task GetAllAsync_IsAuthenticated_ReturnsToDos()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetAllToDos);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<List<API.Dto.ToDo>>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetAllAsync_IsNotAuthenticated_ReturnsUnauthorizedWithMessage()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetAllToDos);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("You do not have permission. Please, log in first");
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_Exists_ReturnsOkWithMessage()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetToDoById + "1");

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();
            content!.Message.Should().Be("Got to-do successfully");
        }

        [Fact]
        public async Task GetByIdAsync_Exists_ReturnsCorrectToDo()
        {
            // Arrange
            const int id = 1;

            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetToDoById + id);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNull();
            content.Data.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetByIdAsync_NotExist_ReturnsNotFoundWithMessage()
        {
            // Arrange
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetToDoById + "99");

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            content.Should().NotBeNull();
            content!.Message.Should().Be("Resource not found");
        }

        [Fact]
        public async Task GetByIdAsync_NotAuthenticated_ReturnsUnauthorizedWithMessage()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync(ApiRoute.GetToDoById + "1");

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();
            content!.Message.Should().Be(ResponseMessage.Unauthorized);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ValidModel_ReturnsOkWithMessage()
        {
            // Arrange
            const int id = 1;

            var model = new UpdateToDoModel
            {
                Title = "updated title",
                Description = "updated description",
                Completed = false,
                Deadline = DateTimeOffset.UtcNow
            };

            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PutAsJsonAsync(ApiRoute.UpdateToDo + id, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Resource updated successfully");
        }

        [Fact]
        public async Task UpdateAsync_ValidModel_ReturnsUpdatedToDo()
        {
            // Arrange
            const int id = 1;

            var model = new UpdateToDoModel
            {
                Title = "updated title",
                Description = "updated description",
                Completed = false,
                Deadline = DateTimeOffset.UtcNow
            };
            
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PutAsJsonAsync(ApiRoute.UpdateToDo + id, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<API.Dto.ToDo>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNull();

            content.Data.Id.Should().Be(id);
            content.Data.Title.Should().Be(model.Title);
            content.Data.Description.Should().Be(model.Description);
            content.Data.Completed.Should().Be(model.Completed);
            content.Data.Deadline.Should().Be(model.Deadline);
        }

        [Fact]
        public async Task UpdateAsync_InvalidModel_ReturnsBadRequestWithMessageAndErrors()
        {
            // Arrange
            const int id = 1;

            var model = new UpdateToDoModel();

            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PutAsJsonAsync(ApiRoute.UpdateToDo + id, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be("One or more validation errors occurred");

            content.Errors.Should().ContainSingle(e =>
                e.Property == "Title" &&
                e.Messages.Contains("'Title' is required")
            );
        }
        
        [Fact]
        public async Task UpdateAsync_NotAuthenticated_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            const int id = 1;

            var model = new UpdateToDoModel();

            // Act
            var response = await _httpClient.PutAsJsonAsync(ApiRoute.UpdateToDo + id, model);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.Unauthorized);
        }
        
        [Fact]
        public async Task UpdateAsync_ToDoNotExist_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            const int id = 99;

            var model = new UpdateToDoModel
            {
                Title = "updated title",
                Description = "updated description",
                Completed = false,
                Deadline = DateTimeOffset.UtcNow
            };
            
            _httpClient.Authenticate();

            // Act
            var response = await _httpClient.PutAsJsonAsync(ApiRoute.UpdateToDo + id, model);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.NotFound);
        }

        #endregion
    }
}