using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using ToDo.API.Services;
using ToDo.UnitTests.TestHelpers;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class CookieServiceTests
    {
        private readonly HttpContext _httpContext;

        public CookieServiceTests()
        {
            _httpContext = new DefaultHttpContext();
        }

        private CookieService CreateService()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);

            httpContextAccessorMock
                .SetupGet(x => x.HttpContext)
                .Returns(_httpContext);

            return new CookieService(httpContextAccessorMock.Object);
        }

        #region Add

        [Fact]
        public void Add_AddsCookieToResponse()
        {
            // Arrange
            const string key = "key";
            const string value = "value";

            var sut = CreateService();

            // Act
            sut.Add(key, value);

            var cookieValueFromResponse = _httpContext.GetCookieValueFromResponse(key);

            // Assert
            cookieValueFromResponse.Should().NotBeNullOrWhiteSpace().And.Be(value);
        }

        [Fact]
        public void Add_CookieHasCorrectOptions()
        {
            // Arrange
            const string key = "key";
            const string expectedOptions = "Secure; SameSite=None; HttpOnly";

            var sut = CreateService();

            // Act
            sut.Add(key, "value");

            var cookieFromResponse = _httpContext.GetCookieFromResponse(key);

            // Assert
            cookieFromResponse.Should().NotBeNullOrWhiteSpace().And.ContainEquivalentOf(expectedOptions);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_AddsExpiredCookieToResponse()
        {
            // Arrange
            const string key = "key";

            var sut = CreateService();

            // Act
            sut.Delete(key);

            var cookieFromResponse = _httpContext.GetCookieFromResponse(key);

            // Assert
            cookieFromResponse.Should()
                .NotBeNullOrWhiteSpace().And
                .StartWithEquivalentOf("key=; expires=Thu, 01 Jan 1970 00:00:00 GMT;");
        }

        [Fact]
        public void Delete_HasCorrectOptions()
        {
            // Arrange
            const string key = "key";
            const string expectedOptions = "Secure; SameSite=None; HttpOnly";

            var sut = CreateService();

            // Act
            sut.Delete(key);

            var cookieFromResponse = _httpContext.GetCookieFromResponse(key);

            // Assert
            cookieFromResponse.Should()
                .NotBeNullOrWhiteSpace().And
                .EndWithEquivalentOf(expectedOptions);
        }

        #endregion

        #region GetValue

        [Fact]
        public void GetValue_CookieExists_ReturnsCookieValue()
        {
            // Arrange
            const string key = "key";
            const string value = "value";

            _httpContext.AddCookieToRequest(key, value);

            var sut = CreateService();

            // Act
            var cookieValue = sut.GetValue(key);

            // Assert
            cookieValue.Should().Be(value);
        }

        [Fact]
        public void GetValue_CookieNotExist_ReturnsNull()
        {
            // Arrange
            var sut = CreateService();

            // Act
            var cookieValue = sut.GetValue("");

            // Assert
            cookieValue.Should().BeNull();
        }

        #endregion
    }
}