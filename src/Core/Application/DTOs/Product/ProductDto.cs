using Domain.Entities;

namespace Application.DTOs.User
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public decimal DiscountRate { get; set; }
        public string Category { get; set; }

        public static ProductDto Create(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                Name = product.Name,
                ImagePath = product.ImagePath.Replace("\\", "/"),
                Price = product.Price,
                MinimumQuantity = product.MinimumQuantity,
                DiscountRate = product.DiscountRate,
                Category = product.Category
            };
        }
        public static List<ProductDto> Create(List<Product> products)
        {
            return products.Select(p => Create(p)).ToList();
        }
    }
}
