using System;

namespace ToDo.API.Dto
{
    public class CreateToDo
    {
        public int UserId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTimeOffset? Deadline { get; init; }
    }
}