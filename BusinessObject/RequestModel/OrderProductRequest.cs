using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class OrderProductRequest
    {
        public List<int> OrderId { get; set; } = new List<int>();
        public List<int> ProductId { get; set; } = new List<int>();
    }
}
