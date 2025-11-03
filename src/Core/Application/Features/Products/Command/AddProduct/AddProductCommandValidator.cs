
namespace Application.Features.Products.Command.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator() { 
            
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

            RuleFor(x => x.DiscountRate)
                .InclusiveBetween(0, 100).WithMessage("Discount rate must be between 0 and 100.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category Name is required.");

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product code is required.")
                .MaximumLength(50).WithMessage("Product code cannot exceed 50 characters.");
        }
    }
}
