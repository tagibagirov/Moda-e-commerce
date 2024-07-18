using FluentValidation;

namespace ModaECommerce.Models.Validator
{
    public class CargoValidator : AbstractValidator<Cargo>
    {
        public CargoValidator()
        {
            RuleFor(x => x.CargoUserName).NotEmpty().WithMessage("Name is required.").MaximumLength(50).WithMessage("The length of name must be 50 characters or fewer");
            RuleFor(x => x.CargoUserSurname).NotEmpty().WithMessage("Surname is required.").MaximumLength(50).WithMessage("The length of surname must be 50 characters or fewer");
            RuleFor(x => x.CargoUserPhone).NotEmpty().WithMessage("Phone is required.").MaximumLength(30).WithMessage("The length of phone number must be 30 characters or fewer");
            RuleFor(x => x.CargoUserEmail).NotEmpty().WithMessage("Email is required.").MaximumLength(60).WithMessage("The length of email must be 60 characters or fewer");
            RuleFor(x => x.CargoAddressCountry).NotEmpty().WithMessage("Country is required.").MaximumLength(50).WithMessage("The length of country must be 50 characters or fewer");
            RuleFor(x => x.CargoAddressCity).NotEmpty().WithMessage("City is required.").MaximumLength(50).WithMessage("The length of city must be 50 characters or fewer");
            RuleFor(x => x.CargoAddressDistrict).NotEmpty().WithMessage("District is required.").MaximumLength(50).WithMessage("The length of district must be 50 characters or fewer");
            RuleFor(x => x.CargoAddressStreet).NotEmpty().WithMessage("Street is required.").MaximumLength(50).WithMessage("The length of street must be 50 characters or fewer");
            RuleFor(x => x.CargoAddressHouse).NotEmpty().WithMessage("House is required.").MaximumLength(10).WithMessage("The length of house must be 10 characters or fewer");
            RuleFor(x => x.CargoAddressApartment).NotEmpty().WithMessage("Apartment is required.").MaximumLength(10).WithMessage("The length of apartment must be 10 characters or fewer");
            RuleFor(x => x.CargoNotes).MaximumLength(300).WithMessage("The length of notes must be 300 characters or fewer");

        }
    }
}
