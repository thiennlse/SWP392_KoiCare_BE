using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class ProductResponseModel
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double Cost { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Origin { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double Productivity { get; set; }

        [Required]
        [StringLength(250)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue)]
        public int InStock { get; set; }
    }
}
