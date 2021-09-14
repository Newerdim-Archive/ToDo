namespace ToDo.API.Dto
{
    public class ExternalTokenPayload
    {
        public string UserId { get; init; }
        public string Email { get; init; }
        public string Username { get; init; }
        public string ProfilePictureUrl { get; init; }
    }
}