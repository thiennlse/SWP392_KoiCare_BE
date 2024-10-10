using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Fish : BaseEntity
    {
        [Required]
        public int PoolId { get; set; }

        [Required]
        public int FoodId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Image { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double Size { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Weight { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Age { get; set; }

        [Required]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Origin { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual Food? Food { get; set; } 
        [JsonIgnore]
        public virtual Pool? Pool { get; set; } = null!;
    }
}
