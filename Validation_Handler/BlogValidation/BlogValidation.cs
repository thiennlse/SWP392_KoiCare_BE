using BusinessObject.RequestModel;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation_Handler.BlogValidation
{
    public class BlogValidation : AbstractValidator<BlogRequestModel>
    {
        public BlogValidation() 
        {
            RuleFor(x => x.MemberId)
              .NotNull().WithMessage("please input memberId")
              .GreaterThan(0).WithMessage("please input memberId biggest than 0");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("title is not empty");
            RuleFor(x => x.Content)
               .NotEmpty().WithMessage("Content is not empty");
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("status is not empty");
            RuleFor(x => x.DateOfPublish)
                 .NotEmpty().WithMessage("please input date");

        }

    }
}
