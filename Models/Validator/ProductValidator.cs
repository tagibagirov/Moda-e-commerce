using FluentValidation;

namespace ModaECommerce.Models.Validator
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("The length of name must be 100 characters or fewer");

            RuleFor(x => x.ProductWeight).NotEmpty().WithMessage("Weight is required.").GreaterThan(0).WithMessage("Weight must be a positive number.");

            RuleFor(x => x.ProductGender).NotEmpty().WithMessage("Gender is required.").MaximumLength(10).WithMessage("The length of gender must be 10 characters or fewer");

            RuleFor(x => x.ProductBrendId).NotNull().WithMessage("Brand is required.");

            RuleFor(x => x.ProductAbout).NotEmpty().WithMessage("Description is required.").MaximumLength(1000).WithMessage("The length of description must be 1000 characters or fewer");

            RuleFor(x => x.ProductCategoryId).NotNull().WithMessage("Category is required.");

            RuleFor(x => x.ProductColorId).NotNull().WithMessage("Color is required.");

            RuleFor(x => x.ProductCountry).NotEmpty().WithMessage("Country is required.").MaximumLength(50).WithMessage("The length of description must be 50 characters or fewer"); 

            RuleFor(x => x.ProductPrice).NotEmpty().WithMessage("Price is required.").GreaterThan(0).WithMessage("Price must be a positive number.");

            RuleFor(x => x.ProductSize).NotEmpty().WithMessage("Size is required.");

            RuleFor(x => x.ProductYear).NotEmpty().WithMessage("Year is required.");
        }
    }
}
