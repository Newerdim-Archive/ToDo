using FluentAssertions;
using ToDo.API.Dto;
using ToDo.API.Extensions;
using Xunit;

namespace ToDo.UnitTests.Extensions
{
    public class ExternalTokenPayloadExtensionsTests
    {
        #region HasProfileInformation

        [Fact]
        public void HasProfileInformation_HasAllInformation_ReturnsTrue()
        {
            // Arrange
            var tokenPayload = new ExternalTokenPayload
            {
                Username = "user1",
                Email = "user1@mail.com",
                ProfilePictureUrl = "url"
            };

            // Act
            var result = tokenPayload.HasProfileInformation();

            // Assert
            result.Should().BeTrue();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void HasProfileInformation_UsernameIsNullOrEmpty_ReturnsFalse(string username)
        {
            // Arrange
            var tokenPayload = new ExternalTokenPayload
            {
                Username = username,
                Email = "user1@mail.com",
                ProfilePictureUrl = "www.example/1"
            };

            // Act
            var result = tokenPayload.HasProfileInformation();

            // Assert
            result.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void HasProfileInformation_EmailIsNullOrEmpty_ReturnsFalse(string email)
        {
            // Arrange
            var tokenPayload = new ExternalTokenPayload
            {
                Username = "user1",
                Email = email,
                ProfilePictureUrl = "www.example/1"
            };

            // Act
            var result = tokenPayload.HasProfileInformation();

            // Assert
            result.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void HasProfileInformation_ProfilePictureUrlIsNullOrEmpty_ReturnsFalse(string profilePictureUrl)
        {
            // Arrange
            var tokenPayload = new ExternalTokenPayload
            {
                Username = "user1",
                Email = "user1@mail.com",
                ProfilePictureUrl = profilePictureUrl
            };

            // Act
            var result = tokenPayload.HasProfileInformation();

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}