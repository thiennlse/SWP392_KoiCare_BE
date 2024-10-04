using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class OrderResponseModel
    {
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
    }
}
