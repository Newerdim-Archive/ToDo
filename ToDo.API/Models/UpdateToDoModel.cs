using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToDo.API.Const;

namespace ToDo.API.Models
{
    public class UpdateToDoModel
    {
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public string Title { get; init; }

        public string Description { get; init; }

        public DateTimeOffset? Deadline { get; init; }

        [JsonProperty(Required = Required.Always)]
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public bool Completed { get; init; }
    }
}