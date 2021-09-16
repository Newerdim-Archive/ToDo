using ToDo.API.Dto;

namespace ToDo.API.Extensions
{
    public static class ExternalTokenPayloadExtensions
    {
        public static bool HasProfileInformation(this ExternalTokenPayload tokenPayload)
        {
            if (string.IsNullOrWhiteSpace(tokenPayload.Username))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(tokenPayload.Email))
            {
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(tokenPayload.ProfilePictureUrl))
            {
                return false;
            }

            return true;
        }
    }
}