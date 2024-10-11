using BusinessObject.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Fish
{
    public class FishValidation : AbstractValidator<FishRequestModel>
    {
        public FishValidation()
        {
            RuleFor(x => x.PoolId)
                .GreaterThan(0).WithMessage("please input PoolId greater than 0")
                .NotNull().WithMessage("this Pool id must not be null");
            RuleFor(x => x.FoodId)
                .GreaterThan(0).WithMessage("please unput Food Id greater than 0")
                .NotNull().WithMessage("this food id must not be null");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("please input Name");
            RuleFor(x => x.Size)
                .NotNull().WithMessage("please input size")
                .GreaterThan(0).WithMessage("size must greater than 0");
            RuleFor(x => x.Weight)
                .NotNull().WithMessage("please input weight")
                .GreaterThan(0).WithMessage("weight must greater than 0");
            RuleFor(x => x.Dob)
                .NotNull().WithMessage("Age of fish can not be null")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Not a valid date");
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("please input gender");
            RuleFor(x => x.Origin)
                .NotEmpty().WithMessage("please input origin");
        }
    }
}
