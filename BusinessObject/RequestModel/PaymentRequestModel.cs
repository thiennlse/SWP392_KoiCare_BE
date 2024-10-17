using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class PaymentRequestModel
    {
        public List<int> orderId { get; set; }

        public double TotalCost { get; set; }

        public string cancelUrl { get; set; }

        public string returnUrl { get; set; }
    }
}
