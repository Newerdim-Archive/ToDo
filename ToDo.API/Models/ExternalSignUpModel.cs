using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToDo.API.Const;
using ToDo.API.Enum;

namespace ToDo.API.Models
{
    public class ExternalSignUpModel
    {
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public string Token { get; init; }

        [JsonProperty(Required = Required.Always)]
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public ExternalAuthProvider Provider { get; init; }
    }
}