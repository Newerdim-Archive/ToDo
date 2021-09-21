using System;
using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Entities
{
    public class ToDo : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        public DateTimeOffset? Deadline { get; set; }

        public bool Completed { get; set; }

        public int UserId { get; set; }
    }
}