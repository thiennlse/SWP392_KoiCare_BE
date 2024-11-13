using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Member : BaseEntity
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Blog>? Blogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<Pool>? Pools { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<UserSubcriptions>? UserSubcriptions { get; set; } = new List<UserSubcriptions>();
    }
}
