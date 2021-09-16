using System.ComponentModel.DataAnnotations;
using ToDo.API.Const;
using ToDo.API.Enum;

namespace ToDo.API.Models
{
    public class ExternalLogInModel
    {
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public string Token { get; init; }
        
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public ExternalAuthProvider Provider { get; init; }
    }
}