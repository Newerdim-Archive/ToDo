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
        private readonly HttpContext _httpContext = new DefaultHttpContext();
        private readonly CookieService _sut;

        public CookieServiceTests()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);

            httpContextAccessorMock
                .SetupGet(x => x.HttpContext)
                .Returns(_httpContext);

            _sut = new CookieService(httpContextAccessorMock.Object);
        }

        #region Add

        [Fact]
        public void Add_AddsCookieToResponse()
        {
            // Arrange
            const string key = "key";
            const string value = "value";

            // Act
            _sut.Add(key, value);

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

            // Act
            _sut.Add(key, "value");

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
            
            // Act
            _sut.Delete(key);

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

            // Act
            _sut.Delete(key);

            var cookieFromResponse = _httpContext.GetCookieFromResponse(key);

            // Assert
            cookieFromResponse.Should()
                .NotBeNullOrWhiteSpace().And
                .EndWithEquivalentOf(expectedOptions);

        }

        #endregion
    }
}