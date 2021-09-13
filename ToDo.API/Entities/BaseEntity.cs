using System;
using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Entities
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
        
        [Required]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}