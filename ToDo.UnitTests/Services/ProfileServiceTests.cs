using System.Threading.Tasks;
using FluentAssertions;
using ToDo.API.Services;
using ToDo.API.Services.Implementations;
using ToDo.UnitTests.DataFixture;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class ProfileServiceTests : IClassFixture<SeededDataFixture>
    {
        private readonly ProfileService _sut;

        public ProfileServiceTests(SeededDataFixture fixture)
        {
            _sut = new ProfileService(fixture.Context);
        }

        #region GetByUserId

        [Fact]
        public async Task GetByUserIdAsync_UserExists_ReturnsProfile()
        {
            // Arrange

            // Act
            var profile = await _sut.GetByUserIdAsync(1);

            // Assert
            profile.Should().NotBeNull();

            profile.Username.Should().Be("user1");
            profile.Email.Should().Be("user1@mail.com");
            profile.ProfilePictureUrl.Should().Be("www.example.com/picture/1");
        }

        [Fact]
        public async Task GetByUserIdAsync_UserNotExist_ReturnsNull()
        {
            // Arrange

            // Act
            var profile = await _sut.GetByUserIdAsync(999);

            // Assert
            profile.Should().BeNull();
        }

        #endregion
    }
}