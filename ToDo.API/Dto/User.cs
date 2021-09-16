using ToDo.API.Enum;

namespace ToDo.API.Dto
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string ProfilePictureUrl { get; init; }
        public string ExternalId { get; init; }
        public ExternalAuthProvider Provider { get; init; }
    }
}