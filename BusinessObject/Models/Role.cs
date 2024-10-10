using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            Members = new HashSet<Member>();
        }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual ICollection<Member>? Members { get; set; }
    }
}
