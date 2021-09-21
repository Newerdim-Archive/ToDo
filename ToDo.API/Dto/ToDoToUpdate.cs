using System;

namespace ToDo.API.Dto
{
    public class ToDoToUpdate
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTimeOffset? Deadline { get; init; }
        public bool Completed { get; init; }
    }
}