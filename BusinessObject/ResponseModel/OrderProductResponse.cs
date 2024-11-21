using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class OrderProductResponse
    {
        public double Amount { get; set; }
        public List<int> ProductId {  get; set; } = new List<int>();
    }
}
