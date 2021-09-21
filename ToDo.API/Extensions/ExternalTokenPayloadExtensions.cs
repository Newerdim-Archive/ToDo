using ToDo.API.Dto;

namespace ToDo.API.Extensions
{
    public static class ExternalTokenPayloadExtensions
    {
        public static bool HasProfileInformation(this ExternalTokenPayload tokenPayload)
        {
            return !string.IsNullOrWhiteSpace(tokenPayload.ProfilePictureUrl) &&
                   !string.IsNullOrWhiteSpace(tokenPayload.Username) && 
                   !string.IsNullOrWhiteSpace(tokenPayload.Email);
        }
    }
}