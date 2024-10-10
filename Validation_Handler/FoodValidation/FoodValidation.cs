using BusinessObject.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation_Handler.FoodValidation
{
    public class FoodValidation : AbstractValidator<FoodRequestModel>
    {
        public FoodValidation() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("please input Name of food ");
            RuleFor(x => x.Weight)
                .NotNull().WithMessage("please input Weight of Food ")
                .GreaterThan(0).WithMessage("weight must be biggest than 0");
        }

    }
}
