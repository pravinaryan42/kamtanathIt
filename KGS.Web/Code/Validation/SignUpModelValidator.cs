
using FluentValidation;
using KGS.Core;
using KGS.Dto;
using KGS.Web.Models;

namespace KGS.Web.Code.Validation
{
    public class SignUpModelValidator : AbstractValidator<SignUpModel>
    {
        public SignUpModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("*required");
            RuleFor(x => x.LastName).NotNull().WithMessage("*required");
            RuleFor(x => x.EmailAddress).EmailAddress().NotNull().WithMessage("*required");
            RuleFor(x => x.Password).NotNull().WithMessage("*required").Matches(ApplicationConstant.PasswordPattern).WithMessage("Password should contain at least 8 character with 1 block letter, 1 small letter and a special character.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("*required").Equal(x => x.Password).WithMessage("*Passwords should match");
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("*required");
            RuleFor(x => x.ConfirmPassword).NotNull().WithMessage("*required");


        }
    }
}