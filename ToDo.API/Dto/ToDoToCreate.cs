using System;

namespace ToDo.API.Dto
{
    public class ToDoToCreate
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTimeOffset? Deadline { get; init; }
    }
}