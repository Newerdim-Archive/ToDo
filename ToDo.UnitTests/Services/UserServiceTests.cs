using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Dto;
using ToDo.API.Enum;
using ToDo.API.Helpers;
using ToDo.API.Services;
using ToDo.UnitTests.DataFixture;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class UserServiceTests : IClassFixture<SeededDataFixture>
    {
        private readonly UserService _sut;
        private readonly DataContext _context;

        public UserServiceTests(SeededDataFixture fixture)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MapperProfile>();
            });

            var mapper = new Mapper(mapperConfiguration);
            
            _context = fixture.Context;
            
            _sut = new UserService(_context, mapper);
        }

        #region GetByExternalIdAsync

        [Fact]
        public async Task GetByExternalIdAsync_UserExists_ReturnsCorrectUser()
        {
            // Arrange
            const string externalId = "1";
            const ExternalAuthProvider provider = ExternalAuthProvider.Google;

            // Act
            var user = await _sut.GetByExternalIdAsync(externalId, provider);

            // Assert
            user.Should().NotBeNull();

            user.ExternalId.Should().Be(externalId);
            user.Provider.Should().Be(provider);

            user.Id.Should().Be(1);
            user.Email.Should().Be("user1@mail.com");
            user.Username.Should().Be("user1");
            user.ProfilePictureUrl.Should().Be("www.example.com/picture/1");
        }
        
        [Fact]
        public async Task GetByExternalIdAsync_UserNotExist_ReturnsNull()
        {
            // Arrange

            // Act
            var user = await _sut.GetByExternalIdAsync("99", ExternalAuthProvider.Google);

            // Assert
            user.Should().BeNull();
        }
        
        #endregion

        #region ExistsByExternalIdAsync

        [Fact]
        public async Task ExistsByExternalIdAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            
            // Act
            var exists = await _sut.ExistsByExternalIdAsync("1", ExternalAuthProvider.Google);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByExternalIdAsync_UserNotExist_ReturnsFalse()
        {
            // Arrange
            
            // Act
            var exists = await _sut.ExistsByExternalIdAsync("999", ExternalAuthProvider.Google);

            // Assert
            exists.Should().BeFalse();
        }
        
        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ReturnCorrectUser()
        {
            // Arrange
            var createUser = new Fixture().Create<CreateUser>();

            // Act
            var createdUser = await _sut.CreateAsync(createUser);

            // Assert
            createdUser.Should().NotBeNull();

            createdUser.Id.Should().Be(3);
            createdUser.Username.Should().Be(createUser.Username);
            createdUser.Email.Should().Be(createUser.Email);
            createdUser.ExternalId.Should().Be(createUser.ExternalId);
            createdUser.Provider.Should().Be(createUser.Provider);
            createdUser.ProfilePictureUrl.Should().Be(createUser.ProfilePictureUrl);
        }

        #endregion
    }
}