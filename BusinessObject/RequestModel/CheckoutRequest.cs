using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class CheckoutRequest
    {
        public List<PaymentRequestModel> OrderRequest { get; set; }
        public int? SubscriptionId { get; set; }
        public string CancelUrl { get; set; }
        public string ReturnUrl { get; set; }
    }
}
