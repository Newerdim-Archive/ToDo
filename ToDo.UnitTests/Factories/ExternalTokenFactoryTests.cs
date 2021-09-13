using System;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Apis.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using ToDo.API.Enum;
using ToDo.API.Factories;
using ToDo.API.Helpers;
using ToDo.API.Services;
using ToDo.API.Wrappers;
using Xunit;

namespace ToDo.UnitTests.Factories
{
    public class ExternalTokenFactoryTests
    {
        private readonly ExternalTokenFactory _sut;

        public ExternalTokenFactoryTests()
        {
            var googleTokenService = CreateGoogleTokenService();

            var serviceProvider = new ServiceCollection()
                .AddScoped(_ => googleTokenService)
                .BuildServiceProvider();

            _sut = new ExternalTokenFactory(serviceProvider);
        }

        private static GoogleTokenService CreateGoogleTokenService()
        {
            var googleJsonWebSignatureWrapperMock = new Mock<IGoogleJsonWebSignatureWrapper>();

            googleJsonWebSignatureWrapperMock
                .Setup(_ => _.ValidateAsync(
                    It.IsAny<string>(),
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>())
                )
                .ReturnsAsync(new GoogleJsonWebSignature.Payload());

            var googleAuthSettingsOptionsMock = new Mock<IOptions<GoogleAuthSettings>>();

            googleAuthSettingsOptionsMock
                .SetupGet(_ => _.Value)
                .Returns(new GoogleAuthSettings());

            return new GoogleTokenService(
                googleJsonWebSignatureWrapperMock.Object,
                googleAuthSettingsOptionsMock.Object);
        }

        #region ValidateAsync

        [Fact]
        public async Task ValidateAsync_ProviderExists_ReturnsPayload()
        {
            // Arrange

            // Act
            var payload = await _sut.ValidateAsync("token", ExternalAuthProvider.Google);

            // Assert
            payload.Should().NotBeNull();
        }

        [Fact]
        public async Task ValidateAsync_ProviderNotExist_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var sut = new ExternalTokenFactory(serviceProvider);

            // Act
            Func<Task> act = async () => await sut.ValidateAsync("token", ExternalAuthProvider.Google);

            // Assert
            await act.Should().ThrowAsync<NullReferenceException>();
        }

        #endregion
    }
}