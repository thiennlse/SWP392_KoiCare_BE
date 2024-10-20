using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Order : BaseEntity
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double TotalCost { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime CloseDate { get; set; }

        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual Member? Member { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Product { get; set; }
    }
}
