using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Subcriptions : BaseEntity
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public ICollection<UserSubcriptions> UserSubcriptions { get; set; } = new List<UserSubcriptions>();
    }
}
