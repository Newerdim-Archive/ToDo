using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using ToDo.API.Const;
using ToDo.API.Filters;
using ToDo.API.Responses;
using Xunit;

namespace ToDo.UnitTests.Filters
{
    public class ValidationFilterTests
    {
        private readonly ValidationFilter _sut = new();
        
        private readonly ModelStateDictionary _modelState = new();
        private readonly ActionExecutingContext _actionExecutingContext;

        public ValidationFilterTests()
        {
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor(),
                _modelState
            );
            
            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);
        }
        
        #region OnActionExecuting

        [Fact]
        public void OnActionExecuting_ModelStateIsInvalid_ReturnsBadRequestObjectResultWithCorrectData()
        {
            // Arrange
            const string property = "property";
            const string validationErrorMessage = "'{0}' is required";
            
            const string expectedMessage = "'property' is invalid";
            
            _modelState.AddModelError(property, validationErrorMessage);

            //Act
            _sut.OnActionExecuting(_actionExecutingContext);

            var result = _actionExecutingContext.Result as BadRequestObjectResult;
            
            var content = result?.Value as ValidationErrorResponse;

            //Assert
            content.Should().NotBeNull();
            
            content!.Message.Should().Be(ResponseMessage.ValidationError);
            
            content.Errors.Should().ContainSingle(x => 
                x.Property == property &&
                x.Messages.Contains(expectedMessage)
            );
        }

        [Fact]
        public void OnActionExecuting_ModelStateIsValid_ReturnsNullResult()
        {
            // Arrange
            
            //Act
            _sut.OnActionExecuting(_actionExecutingContext);

            var result = _actionExecutingContext.Result;
            
            //Assert
            result.Should().BeNull();
        }

        #endregion
    }
}