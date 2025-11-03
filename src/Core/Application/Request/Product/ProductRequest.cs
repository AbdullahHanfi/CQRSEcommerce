
namespace Application.Request.Product;

public class ProductRequst
{
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public IFormFile Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal DiscountRate { get; set; }
    public string Category { get; set; }

}