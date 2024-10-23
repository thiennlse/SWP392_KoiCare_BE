using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class Product : BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Cost { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public double Productivity { get; set; }
        public string Code { get; set; } = string.Empty;
        public int InStock { get; set; }

        [JsonIgnore]
        public virtual Member? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
