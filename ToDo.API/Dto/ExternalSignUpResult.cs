using ToDo.API.Enum;

namespace ToDo.API.Dto
{
    public class ExternalSignUpResult
    {
        public User CreatedUser { get; set; }
        public ExternalSignUpResultMessage Message { get; set; }
    }
}