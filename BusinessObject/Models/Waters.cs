using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Waters : BaseEntity
    {
        public Waters()
        {
            Pools = new HashSet<Pool>();
        }
        [Required]
        [Range(-273.15, double.MaxValue)]
        public double Temperature { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Salt { get; set; }

        [Required]
        [Range(0, 14)]
        public double Ph { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double O2 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double No2 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double No3 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Po4 { get; set; }
        [JsonIgnore]
        public virtual ICollection<Pool>? Pools { get; set; }
    }
}
