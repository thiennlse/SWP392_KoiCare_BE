using FluentValidation;
using BusinessObject.RequestModel;

namespace Validation_Handler.OrderValidation
{
    public class OrderValidation : AbstractValidator<OrderRequestModel>
    {
        public OrderValidation() 
        {
            RuleFor(x => x.OrderDate).LessThan(x => x.CloseDate).WithMessage("Not valid endate");
        }
    }
}
