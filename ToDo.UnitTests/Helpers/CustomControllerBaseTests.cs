using System.Linq;
using FluentAssertions;
using ToDo.API.Helpers;
using ToDo.API.Responses;
using Xunit;

namespace ToDo.UnitTests.Helpers
{
    public class CustomControllerBaseTests : CustomControllerBase
    {
        #region Ok

        [Fact]
        public void Ok_ReturnsOkWithMessage()
        {
            // Arrange
            const string message = "message";

            // Act
            var result = Ok(message);
            var response = result.Value as BaseResponse;

            // Assert
            response.Should().NotBeNull();

            response!.Message.Should().Be(message);
        }

        #endregion

        #region Ok With Data

        [Fact]
        public void Ok_WithData_ReturnsOkWithDataAndMessage()
        {
            // Arrange
            const string message = "message";
            const string data = "data";

            // Act
            var result = Ok(message, data);
            var response = result.Value as WithDataResponse<string>;

            // Assert
            response.Should().NotBeNull();

            response!.Message.Should().Be(message);
            response.Data.Should().Be(data);
        }

        #endregion

        #region Unauthorized

        [Fact]
        public void Unauthorized_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            const string message = "message";

            // Act
            var result = Unauthorized(message);
            var response = result.Value as BaseResponse;

            // Assert
            response.Should().NotBeNull();

            response!.Message.Should().Be(message);
        }

        #endregion

        #region Conflict

        [Fact]
        public void Conflict_ReturnsConflictWithMessage()
        {
            // Arrange
            const string message = "message";

            // Act
            var result = Conflict(message);
            var response = result.Value as BaseResponse;

            // Assert
            response.Should().NotBeNull();

            response!.Message.Should().Be(message);
        }

        #endregion

        #region BadRequest

        [Fact]
        public void BadRequest_ReturnsBadRequestWithValidationError()
        {
            // Arrange
            const string property = "property";
            const string validationErrorMessage = "'{0}' is invalid";

            // Act
            var result = BadRequest(property, validationErrorMessage);
            var response = result.Value as ValidationErrorResponse;

            // Assert
            response.Should().NotBeNull();

            response!.Message.Should().Be("One or more validation errors occurred");

            response.Errors.Should().ContainSingle(x =>
                x.Property == property && 
                x.Messages.Contains("'property' is invalid")
            );
        }
        
        #endregion

        #region NotFound

        [Fact]
        public void NotFound_ReturnsNotFoundWithCorrectErrorResponse()
        {
            // Arrange
            const string message = "message";

            // Act
            var result = NotFound(message);
            var response = result.Value as BaseResponse;

            // Assert
            response.Should().NotBeNull();

            response!.Message.Should().Be(message);
        }

        #endregion
    }
}