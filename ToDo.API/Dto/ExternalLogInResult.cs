using ToDo.API.Enum;

namespace ToDo.API.Dto
{
    public class ExternalLogInResult
    {
        public User User { get; init; }
        public ExternalLogInResultMessage Message { get; init; }
    }
}