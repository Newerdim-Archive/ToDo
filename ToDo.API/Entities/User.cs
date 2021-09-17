using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.API.Enum;

namespace ToDo.API.Entities
{
    public class User : BaseEntity
    {
        [Required] 
        public string Username { get; init; }

        [Required] 
        public string Email { get; init; }

        [Required] 
        public string ProfilePictureUrl { get; init; }

        [Required] 
        public string ExternalId { get; init; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public ExternalAuthProvider Provider { get; init; }
        
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}