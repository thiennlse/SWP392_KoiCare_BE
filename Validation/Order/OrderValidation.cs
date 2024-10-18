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
            RuleFor(o => o.MemberId).NotNull().WithMessage("memberId is not null")
                .GreaterThan(0).WithMessage("memberId must biggest than 0");
            RuleFor(o => o.ProductId).NotNull().WithMessage("productId is not null")
                .GreaterThan(0).WithMessage("productId must biggest than 0");
            RuleFor(o => o.TotalCost).NotNull().WithMessage("totalcost not null")
                .GreaterThan(0).WithMessage("totalCost must biggest than 0");
            RuleFor(o => o.CloseDate).LessThanOrEqualTo(DateTime.Now).WithMessage("close date must lessthan or equal today")
                .NotEmpty().WithMessage("please input closeDate");
            RuleFor(o => o.Code).NotEmpty().WithMessage("Code must not empty");
            RuleFor(o => o.Description).NotEmpty().WithMessage("Description must not empty");
            RuleFor(o => o.Status).NotEmpty().WithMessage("status must not empty");
        }
    }
}
