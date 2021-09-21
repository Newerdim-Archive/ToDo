using System;
using System.ComponentModel.DataAnnotations;
using ToDo.API.Const;

namespace ToDo.API.Models
{
    public class CreateToDoModel
    {
        [Required(ErrorMessage = ValidationErrorMessage.IsRequired)]
        public string Title { get; init; }

        public string Description { get; init; }

        public DateTimeOffset? Deadline { get; init; }
    }
}