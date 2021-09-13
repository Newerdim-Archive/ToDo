using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using ToDo.API.Dto;
using ToDo.API.Enum;
using ToDo.API.Factories;
using ToDo.API.Services;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _sut;

        private readonly Mock<IExternalTokenFactory> _externalTokenFactoryMock;
        private readonly Mock<IUserService> _userService;

        public AuthServiceTests()
        {
            _externalTokenFactoryMock = new Mock<IExternalTokenFactory>(MockBehavior.Strict);
            _userService = new Mock<IUserService>(MockBehavior.Strict);

            _sut = new AuthService(_externalTokenFactoryMock.Object, _userService.Object);
        }

        #region ExternalSignUpAsync

        [Fact]
        public async Task ExternalSignUpAsync_TokenIsValid_ReturnsSuccessMessage()
        {
            // Arrange
            const string token = "token";
            const ExternalAuthProvider provider = ExternalAuthProvider.Google;

            var externalTokenPayload = new Fixture().Create<ExternalTokenPayload>();

            _externalTokenFactoryMock
                .Setup(x => x.ValidateAsync(token, provider))
                .ReturnsAsync(externalTokenPayload);

            _userService
                .Setup(x => x.ExistsByExternalIdAsync(externalTokenPayload.UserId, provider))
                .ReturnsAsync(false);
            
            _userService
                .Setup(x => x.CreateAsync(
                    It.Is<CreateUser>(
                        u => u.Username == externalTokenPayload.Username &&
                             u.Email == externalTokenPayload.Email &&
                             u.ExternalId == externalTokenPayload.UserId &&
                             u.ProfilePictureUrl == externalTokenPayload.ProfilePictureUrl &&
                             u.Provider == provider
                    )
                ))
                .ReturnsAsync(new User());

            // Act
            var result = await _sut.ExternalSignUpAsync(token, provider);

            // Assert
            result.Should().NotBeNull();

            result.Message.Should().Be(ExternalSignUpResultMessage.SignedUpSuccessfully);

            _externalTokenFactoryMock.VerifyAll();
            _userService.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_TokenIsInvalid_ReturnsTokenIsInvalidMessage()
        {
            // Arrange
            const string token = "invalid";
            const ExternalAuthProvider provider = ExternalAuthProvider.Google;

            _externalTokenFactoryMock
                .Setup(x => x.ValidateAsync(token, provider))
                .ReturnsAsync((ExternalTokenPayload) null);

            // Act
            var result = await _sut.ExternalSignUpAsync(token, provider);

            // Assert
            result.Should().NotBeNull();

            result.Message.Should().Be(ExternalSignUpResultMessage.TokenIsInvalid);

            _externalTokenFactoryMock.VerifyAll();
        }

        [Fact]
        public async Task
            ExternalSignUpAsync_TokenNotHaveProfileInformation_ReturnsTokenNotHaveProfileInformationMessage()
        {
            // Arrange
            const string token = "invalid";
            const ExternalAuthProvider provider = ExternalAuthProvider.Google;

            _externalTokenFactoryMock
                .Setup(x => x.ValidateAsync(token, provider))
                .ReturnsAsync(new ExternalTokenPayload());

            // Act
            var result = await _sut.ExternalSignUpAsync(token, provider);

            // Assert
            result.Should().NotBeNull();

            result.Message.Should().Be(ExternalSignUpResultMessage.TokenNotHaveProfileInformation);

            _externalTokenFactoryMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_UserAlreadyExists_ReturnsUserAlreadyExistsMessage()
        {
            // Arrange
            const string token = "token";
            const ExternalAuthProvider provider = ExternalAuthProvider.Google;

            var externalTokenPayload = new Fixture().Create<ExternalTokenPayload>();

            _externalTokenFactoryMock
                .Setup(x => x.ValidateAsync(token, provider))
                .ReturnsAsync(externalTokenPayload);

            _userService
                .Setup(x => x.ExistsByExternalIdAsync(externalTokenPayload.UserId, provider))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.ExternalSignUpAsync(token, provider);

            // Assert
            result.Should().NotBeNull();

            result.Message.Should().Be(ExternalSignUpResultMessage.UserAlreadyExists);

            _externalTokenFactoryMock.VerifyAll();
            _userService.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_TokenIsValid_ReturnsCreatedUser()
        {
            // Arrange
            const string token = "token";
            const ExternalAuthProvider provider = ExternalAuthProvider.Google;

            var fixture = new Fixture();

            var externalTokenPayload = fixture.Create<ExternalTokenPayload>();
            var createdUser = fixture.Create<User>();

            _externalTokenFactoryMock
                .Setup(x => x.ValidateAsync(token, provider))
                .ReturnsAsync(externalTokenPayload);

            _userService
                .Setup(x => x.ExistsByExternalIdAsync(externalTokenPayload.UserId, provider))
                .ReturnsAsync(false);

            _userService
                .Setup(x => x.CreateAsync(
                    It.Is<CreateUser>(
                        u => u.Username == externalTokenPayload.Username &&
                             u.Email == externalTokenPayload.Email &&
                             u.ExternalId == externalTokenPayload.UserId &&
                             u.ProfilePictureUrl == externalTokenPayload.ProfilePictureUrl &&
                             u.Provider == provider
                    )
                ))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _sut.ExternalSignUpAsync(token, provider);

            // Assert
            result.Should().NotBeNull();
            
            result.CreatedUser.Should().NotBeNull().And.Be(createdUser);

            _externalTokenFactoryMock.VerifyAll();
            _userService.VerifyAll();
        }

        #endregion
    }
}