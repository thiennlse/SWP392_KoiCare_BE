using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class PaymentRequestModel
    {
        public int ProductId { get; set; }
        public double Cost { get; set; }
        public int quantity { get; set; }
    }
}
