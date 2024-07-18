using FluentValidation;

namespace ModaECommerce.Models.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Name is required.").MaximumLength(50).WithMessage("The length of name must be 50 characters or fewer");
            RuleFor(x => x.UserSurname).NotEmpty().WithMessage("Surname is required.").MaximumLength(50).WithMessage("The length of surname must be 50 characters or fewer");
            RuleFor(x => x.UserNickname).NotEmpty().WithMessage("Nickname is required.").MaximumLength(50).WithMessage("The length of nickname must be 50 characters or fewer");
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage("Email is required.").MaximumLength(60).WithMessage("The length of email must be 60 characters or fewer").EmailAddress();
            RuleFor(x => x.UserPhone).NotEmpty().WithMessage("Phone number is required.").MaximumLength(30).WithMessage("The length of phone number must be 30 characters or fewer");
            //RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Password is required.").MaximumLength(50).WithMessage("The length of password must be 50 characters or fewer").MinimumLength(8).WithMessage("The length of password must be 8 characters or more.");
        }
    }
}
