using BusinessObject.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Member
{
    public class MemberValidation : AbstractValidator<MemberRequestModel>
    {

        public MemberValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Required email")
                .EmailAddress().WithMessage("Not a valid email");
        }
    }
}
