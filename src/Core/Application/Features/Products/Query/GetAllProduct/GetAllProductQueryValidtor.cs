
namespace Application.Features.Products.Query.GetAllProduct;

public class GetAllProductQueryValidtor : AbstractValidator<GetAllProductQuery>
{
    public GetAllProductQueryValidtor()
    {
        RuleFor(e => e.page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");
    }
}
