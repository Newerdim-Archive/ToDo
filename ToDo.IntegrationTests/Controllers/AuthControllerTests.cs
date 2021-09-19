using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Google.Apis.Auth;
using Moq;
using ToDo.API;
using ToDo.API.Const;
using ToDo.API.Enum;
using ToDo.API.Models;
using ToDo.API.Responses;
using ToDo.API.Wrappers;
using ToDo.IntegrationTests.Helpers;
using Xunit;

namespace ToDo.IntegrationTests.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<IGoogleJsonWebSignatureWrapper> _googleJsonWebSignatureWrapperMock;
        private readonly HttpClient _httpClient;

        public AuthControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _googleJsonWebSignatureWrapperMock = new Mock<IGoogleJsonWebSignatureWrapper>(MockBehavior.Strict);

            var mockedFixture = factory.ReplaceGoogleJsonWebSignatureWrapper(_googleJsonWebSignatureWrapperMock.Object);

            _httpClient = mockedFixture.CreateClient();
        }

        #region ExternalSignUpAsync

        [Fact]
        public async Task ExternalSignUpAsync_ValidModel_ReturnsOkWithMessage()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "token",
                Provider = ExternalAuthProvider.Google
            };

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    model.Token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ReturnsAsync(CreateGoogleJsonWebSignaturePayload());

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<string>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.SignedUpSuccessfully);

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_ValidModel_ReturnsAccessTokenInResponse()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "token",
                Provider = ExternalAuthProvider.Google
            };

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    model.Token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ReturnsAsync(CreateGoogleJsonWebSignaturePayload);

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<string>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNullOrWhiteSpace();

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_ValidModel_ReturnsRefreshTokenInCookie()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "token",
                Provider = ExternalAuthProvider.Google
            };

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    model.Token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ReturnsAsync(CreateGoogleJsonWebSignaturePayload());

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var refreshToken = response.GetCookie(CookieName.RefreshToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            refreshToken.Should().NotBeNullOrWhiteSpace();

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_TokenIsInvalid_ReturnsBadRequestWithMessageAndErrors()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "invalid",
                Provider = ExternalAuthProvider.Google
            };

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    model.Token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ThrowsAsync(new InvalidJwtException(""));

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.ValidationError);
            content.Errors.Should().ContainSingle(x =>
                x.Property == "Token" &&
                x.Messages.Contains("'Token' is invalid")
            );

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_TokenNotHaveProfileInformation_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "token",
                Provider = ExternalAuthProvider.Google
            };

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    model.Token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload());

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.ValidationError);
            content.Errors.Should().ContainSingle(x =>
                x.Property == "Token" &&
                x.Messages.Contains("'Token' does not have profile information")
            );

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_UserAlreadyExists_ReturnsConflictWithErrorMessage()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "token",
                Provider = ExternalAuthProvider.Google
            };

            var googleJsonWebSignaturePayload = CreateGoogleJsonWebSignaturePayload();
            googleJsonWebSignaturePayload.Subject = "1";

            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(
                    model.Token,
                    It.IsAny<GoogleJsonWebSignature.ValidationSettings>()
                ))
                .ReturnsAsync(googleJsonWebSignaturePayload);

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.UserAlreadyExists);

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ExternalSignUpAsync_InvalidModel_ReturnsBadRequestWithErrorMessage(
            string token)
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = token,
                Provider = ExternalAuthProvider.Google
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be(ResponseMessage.ValidationError);
            content.Errors.Should().ContainSingle(x =>
                x.Property == "Token" &&
                x.Messages.Contains("'Token' is required")
            );

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        #endregion

        #region LogOut

        [Fact]
        public async Task LogOut_ReturnsOkWithMessage()
        {
            // Arrange

            // Act
            var response = await _httpClient.PostAsync(ApiRoute.LogOut, null!);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Logged out successfully");
        }

        [Fact]
        public async Task LogOut_AddsExpiredRefreshToken()
        {
            // Arrange

            // Act
            var response = await _httpClient.PostAsync(ApiRoute.LogOut, null!);

            var refreshToken = response.GetCookie(CookieName.RefreshToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            refreshToken.Should()
                .NotBeNullOrWhiteSpace().And
                .Be("refresh_token=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; secure; samesite=none; httponly");
        }

        #endregion

        #region ExternalLogInAsync

        [Theory, AutoData]
        public async Task ExternalLogInAsync_ModelIsValid_ReturnsOkWithMessage(ExternalLogInModel model)
        {
            // Arrange
            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(model.Token, It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload
                {
                    Subject = "1"
                });

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalLogIn, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<string>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Logged in successfully");

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Theory, AutoData]
        public async Task ExternalLogInAsync_InvalidToken_ReturnsBadRequestWithMessageAndError(ExternalLogInModel model)
        {
            // Arrange
            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(model.Token, It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ThrowsAsync(new InvalidJwtException(""));

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalLogIn, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be("One or more validation errors occurred");
            content!.Errors.Should().ContainSingle(x =>
                x.Property == "Token" &&
                x.Messages.Contains("'Token' is invalid")
            );

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Theory, AutoData]
        public async Task ExternalLogInAsync_UserNotExist_ReturnsUnauthorizedWithMessage(ExternalLogInModel model)
        {
            // Arrange
            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(model.Token, It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload
                {
                    Subject = "9999"
                });

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalLogIn, model);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("User not exist");

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Theory, AutoData]
        public async Task ExternalLogInAsync_ModelIsValid_ReturnsAccessToken(ExternalLogInModel model)
        {
            // Arrange
            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(model.Token, It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload
                {
                    Subject = "1"
                });

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalLogIn, model);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<string>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNullOrWhiteSpace();

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Theory, AutoData]
        public async Task ExternalLogInAsync_ModelIsValid_ReturnsRefreshTokenInCookie(ExternalLogInModel model)
        {
            // Arrange
            _googleJsonWebSignatureWrapperMock
                .Setup(x => x.ValidateAsync(model.Token, It.IsAny<GoogleJsonWebSignature.ValidationSettings>()))
                .ReturnsAsync(new GoogleJsonWebSignature.Payload
                {
                    Subject = "1"
                });

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalLogIn, model);

            var refreshToken = response.GetCookie(CookieName.RefreshToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            refreshToken.Should().NotBeNullOrWhiteSpace();

            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalLogInAsync_ModelIsInvalid_ReturnsBadRequestWithMessageAndError()
        {
            // Arrange
            var model = new ExternalLogInModel();

            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalLogIn, model);

            var content = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();

            content!.Message.Should().Be("One or more validation errors occurred");

            content.Errors.Should().ContainSingle(x =>
                    x.Property == "Token" &&
                    x.Messages.Contains("'Token' is required")
                );
        }

        #endregion

        #region RefreshTokens

        [Fact]
        public async Task RefreshTokens_RefreshTokenIsValid_ReturnsOkWithMessage()
        {
            // Arrange
            _httpClient.AddCookie(CookieName.RefreshToken, RefreshToken.Valid);
            
            // Act
            var response = await _httpClient.GetAsync(ApiRoute.RefreshTokens);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<string>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Tokens have been refreshed successfully");
        }
        
        [Fact]
        public async Task RefreshTokens_RefreshTokenNotExist_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            
            // Act
            var response = await _httpClient.GetAsync(ApiRoute.RefreshTokens);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Refresh token does not exist");
        }

        [Fact]
        public async Task RefreshTokens_RefreshTokenIsInvalid_ReturnsUnauthorizedWithMessage()
        {
            // Arrange
            _httpClient.AddCookie(CookieName.RefreshToken, RefreshToken.Invalid);
            
            // Act
            var response = await _httpClient.GetAsync(ApiRoute.RefreshTokens);

            var content = await response.Content.ReadFromJsonAsync<BaseResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            content.Should().NotBeNull();

            content!.Message.Should().Be("Refresh token is invalid or expired");
        }
        
        [Fact]
        public async Task RefreshTokens_RefreshTokenIsValid_ReturnsNewRefreshTokenInCookies()
        {
            // Arrange
            _httpClient.AddCookie(CookieName.RefreshToken, RefreshToken.Valid);
            
            // Act
            var response = await _httpClient.GetAsync(ApiRoute.RefreshTokens);

            var newRefreshToken = response.GetCookieValue(CookieName.RefreshToken);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            newRefreshToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public async Task RefreshTokens_RefreshTokenIsValid_ReturnsAccessToken()
        {
            // Arrange
            _httpClient.AddCookie(CookieName.RefreshToken, RefreshToken.Valid);
            
            // Act
            var response = await _httpClient.GetAsync(ApiRoute.RefreshTokens);

            var content = await response.Content.ReadFromJsonAsync<WithDataResponse<string>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.Data.Should().NotBeNullOrWhiteSpace();
        }

        #endregion

        private static GoogleJsonWebSignature.Payload CreateGoogleJsonWebSignaturePayload()
        {
            return new Fixture().Create<GoogleJsonWebSignature.Payload>();
        }
    }
}