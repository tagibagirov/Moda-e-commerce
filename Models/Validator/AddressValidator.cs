using FluentValidation;

namespace ModaECommerce.Models.Validator
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.AddressTitle).NotEmpty().WithMessage("Title is required.").MaximumLength(30).WithMessage("The length of title must be 30 characters or fewer");
            RuleFor(x => x.AddressCountry).NotEmpty().WithMessage("Country is required.").MaximumLength(50).WithMessage("The length of country must be 50 characters or fewer");
            RuleFor(x => x.AddressCity).NotEmpty().WithMessage("City is required.").MaximumLength(50).WithMessage("The length of city must be 50 characters or fewer");
            RuleFor(x => x.AddressDistrict).NotEmpty().WithMessage("District is required.").MaximumLength(50).WithMessage("The length of district must be 50 characters or fewer");
            RuleFor(x => x.AddressStreet).NotEmpty().WithMessage("Street is required.").MaximumLength(50).WithMessage("The length of street must be 50 characters or fewer");
            RuleFor(x => x.AddressHouse).NotEmpty().WithMessage("House is required.").MaximumLength(10).WithMessage("The length of house must be 10 characters or fewer");
            RuleFor(x => x.AddressApartment).NotEmpty().WithMessage("Apartment is required.").MaximumLength(10).WithMessage("The length of apartment must be 10 characters or fewer");
        }
    }
}
