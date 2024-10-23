using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class Order : BaseEntity
    {
        public int MemberId { get; set; }
        public double TotalCost { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public virtual Member? Member { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
