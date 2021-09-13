using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Google.Apis.Auth;
using Moq;
using ToDo.API;
using ToDo.API.Const;
using ToDo.API.Dto;
using ToDo.API.Enum;
using ToDo.API.Models;
using ToDo.API.Wrappers;
using ToDo.IntegrationTests.Helpers;
using Xunit;

namespace ToDo.IntegrationTests.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        
        private readonly Mock<IGoogleJsonWebSignatureWrapper> _googleJsonWebSignatureWrapperMock;

        public AuthControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _googleJsonWebSignatureWrapperMock = new Mock<IGoogleJsonWebSignatureWrapper>(MockBehavior.Strict);

            var mockedFixture = factory.ReplaceGoogleJsonWebSignatureWrapper(_googleJsonWebSignatureWrapperMock.Object);
            
            _httpClient = mockedFixture.CreateClient();
        }

        #region ExternalSignUpAsync

        [Fact]
        public async Task ExternalSignUpAsync_ValidModel_ReturnsOk()
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

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_ValidModel_ReturnsSuccessMessageInResponse()
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

            var content = await response.Content.ReadFromJsonAsync<ExternalSignUpResponse>();

            // Assert
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

            var content = await response.Content.ReadFromJsonAsync<ExternalSignUpResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();

            content!.AccessToken.Should().NotBeNullOrWhiteSpace();
            
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
        public async Task ExternalSignUpAsync_TokenIsInvalid_ReturnsBadRequest()
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

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }
        
        [Fact]
        public async Task ExternalSignUpAsync_TokenIsInvalid_ReturnsCorrectErrorMessage()
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

            var errorMessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            errorMessage.Should().Contain(ResponseMessage.TokenIsInvalid);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }
        
        [Fact]
        public async Task ExternalSignUpAsync_TokenNotHaveProfileInformation_ReturnsBadRequest()
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

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }
        
        [Fact]
        public async Task ExternalSignUpAsync_TokenNotHaveProfileInformation_ReturnsCorrectErrorMessage()
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

            var errorMessage = await response.Content.ReadAsStringAsync(); 

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            errorMessage.Should().Contain(ResponseMessage.TokenNotHaveProfileInformation);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }
        
        [Fact]
        public async Task ExternalSignUpAsync_UserAlreadyExists_ReturnsConflict()
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
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Fact]
        public async Task ExternalSignUpAsync_UserAlreadyExists_ReturnsCorrectErrorMessage()
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

            var errorMessage = await response.Content.ReadAsStringAsync(); 

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            errorMessage.Should().Contain(ResponseMessage.UserAlreadyExists);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ExternalSignUpAsync_InvalidModel_ReturnsBadRequest(
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
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }
        
        [Fact]
        public async Task ExternalSignUpAsync_InvalidModel_ReturnsErrorMessage()
        {
            // Arrange
            var model = new ExternalSignUpModel
            {
                Token = "",
                Provider = ExternalAuthProvider.Google
            };
            
            // Act
            var response = await _httpClient.PostAsJsonAsync(ApiRoute.ExternalSignUp, model);

            var errorMessage = await response.Content.ReadAsStringAsync();
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            errorMessage.Should().Contain("Token is required");
            
            _googleJsonWebSignatureWrapperMock.VerifyAll();
        }
        
        #endregion

        private static GoogleJsonWebSignature.Payload CreateGoogleJsonWebSignaturePayload()
        {
            return new Fixture().Create<GoogleJsonWebSignature.Payload>();
        }
    }
}