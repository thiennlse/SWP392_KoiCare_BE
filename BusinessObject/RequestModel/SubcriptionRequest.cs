using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class SubcriptionRequest
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
    }
}
