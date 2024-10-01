using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Member
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Image {  get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(200)]
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
    }
}
