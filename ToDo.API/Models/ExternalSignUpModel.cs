using System.ComponentModel.DataAnnotations;
using ToDo.API.Const;
using ToDo.API.Enum;

namespace ToDo.API.Models
{
    public class ExternalSignUpModel
    {
        [Display(Name = "Token")]
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public string Token { get; init; }
        
        [Required]
        public ExternalAuthProvider Provider { get; init; }
    }
}