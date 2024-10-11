using BusinessObject.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Order
{
    public class OrderValidation : AbstractValidator<OrderRequestModel>
    {
        public OrderValidation()
        {

        }
    }
}
