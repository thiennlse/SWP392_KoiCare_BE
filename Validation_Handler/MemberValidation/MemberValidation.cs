using FluentValidation;
using BusinessObject.RequestModel;

namespace Validation_Handler.MemberValidation
{
    public class MemberValidation :AbstractValidator<MemberRequestModel>
    {

        public MemberValidation() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Required email")
                .EmailAddress().WithMessage("Not a valid email");
        }

    }
}
