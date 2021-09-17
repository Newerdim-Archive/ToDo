using System;

namespace ToDo.API.Models
{
    public class CreateToDoModel
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTimeOffset? Deadline { get; init; }
    }
}