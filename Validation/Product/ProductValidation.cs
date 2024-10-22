using BusinessObject.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Product
{
    public class ProductValidation : AbstractValidator<ProductRequestModel>
    {
        public ProductValidation() 
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required");
            RuleFor(p=> p.Image)
                .NotEmpty().WithMessage("Image is required");
            RuleFor(p => p.Cost)
                .GreaterThanOrEqualTo(1).WithMessage("Cost cannot lower than 1")
                .NotEmpty().WithMessage("Cost is required");
            RuleFor(p => p.Origin)
                .NotEmpty().WithMessage("Origin is required");
            RuleFor(p => p.Productivity)
                .GreaterThanOrEqualTo(1).WithMessage("Productivity cannot lower than 1")
                .NotEmpty().WithMessage("Productivity is required");
            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("Code is required");
            RuleFor(p => p.InStock)
                .GreaterThanOrEqualTo(1).WithMessage("Instock cannot lower than 1")
                .NotEmpty().WithMessage("InStock is required");
        }
    }
}
