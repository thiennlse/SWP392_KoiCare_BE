using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class UserSubcriptions : BaseEntity
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual Member Member { get; set; }

        public int SubcriptionId { get; set; }
        public virtual Subcriptions Subcriptions { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
