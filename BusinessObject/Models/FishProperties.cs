using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class FishProperties : BaseEntity
    {
        public int FishId { get; set; }
        public DateTime Date { get; set; }
        public double Size { get; set; }
        public double Weight { get; set; }
        [JsonIgnore]
        public virtual Fish Fish { get; set; }
    }
}
