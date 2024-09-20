using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double TotalCost { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime CloseDate { get; set; }

        [Required]
        [StringLength(50)]
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
        public virtual Product? Product { get; set; }
    }
}
