using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Moq;
using ToDo.API.Helpers;
using ToDo.API.Services;
using ToDo.API.Services.Implementations;
using ToDo.API.Wrappers;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class GoogleTokenServiceTests
    {
        private readonly GoogleAuthSettings _googleAuthSettings;

        private readonly Mock<IGoogleJsonWebSignatureWrapper> _googleJsonWebSignatureWrapperMock;
        private readonly GoogleTokenService _sut;

        public GoogleTokenServiceTests()
        {
            _googleJsonWebSignatureWrapperMock = new Mock<IGoogleJsonWebSignatureWrapper>(MockBehavior.Strict);

            _googleAuthSettings = new GoogleAuthSettings
            {
                ClientId = "Test google client id"
            };

            var googleAuthSettingsOptions = new Mock<IOptions<GoogleAuthSettings>>(MockBehavior.Strict);

            googleAuthSettingsOptions
                .SetupGet(x => x.Value)
                .Returns(_googleAuthSettings);

            _sut = new GoogleTokenService(
                _googleJsonWebSignatureWrapperMock.Object,
                googleAuthSettingsOptions.Object);
        }

        #region ValidateAsync

        [Fact]
        public async Task ValidateAsync_TokenIsValid_ReturnsPayload()
        {
            // Arrange
            const string token = "valid token";

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(token, It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload());

            // Act
            var payload = await _sut.ValidateAsync(token);

            // Assert
            payload.Should().NotBeNull();

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ValidateAsync_TokenIsInvalid_ReturnsNull()
        {
            // Arrange
            const string token = "valid token";

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ThrowsAsync(new InvalidJwtException(""));

            // Act
            var payload = await _sut.ValidateAsync(token);

            // Assert
            payload.Should().BeNull();

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ValidateAsync_UsesCorrectValidationSettings()
        {
            // Arrange
            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<string>(),
                    It.Is<GoogleJsonWebSignature.ValidationSettings>(
                        s => s.Audience.Contains(_googleAuthSettings.ClientId)
                    )
                ))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload());

            // Act
            await _sut.ValidateAsync("token");

            // Assert
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ValidateAsync_TokenIsValid_ReturnsCorrectPayload()
        {
            // Arrange
            var googlePayload = new Fixture().Create<GoogleJsonWebSignature.Payload>();

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<string>(),
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ReturnsAsync(googlePayload);

            // Act
            var payload = await _sut.ValidateAsync("token");

            // Assert
            payload.Should().NotBeNull();

            payload.UserId.Should().Be(googlePayload.Subject);
            payload.Email.Should().Be(googlePayload.Email);
            payload.Username.Should().Be(googlePayload.Name);
            payload.ProfilePictureUrl.Should().Be(googlePayload.Picture);

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        #endregion
    }
}