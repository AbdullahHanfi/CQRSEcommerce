using Application.Abstractions.Messaging;
using Application.DTOs.User;
using Application.Features.Products.Command.AddProduct;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Tests.Features.Products.Command.AddProduct;

public class AddProductCommandHandler(IRepository<Product> productRepository, IImageService imageService, IUnitOfWork unitOfWork) : ICommandHandler<AddProductCommand, ProductDto>
{
    public async Task<Result<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await productRepository.FindAsync(p => p.ProductCode == request.ProductCode);

        if (existingProduct != null && existingProduct.Count() != 0)
        {
            return await Task.FromResult(Result.Failure<ProductDto>(new("Product with the same code already exists.")));
        }
        var path = await imageService.UploadAsync(request.Image, "wwwroot/products");
        path = path.Replace("wwwroot", string.Empty).Replace("\\", "/");

        var product = new Product
        {
            ProductCode = request.ProductCode,
            Name = request.Name,
            ImagePath = path,
            Price = request.Price,
            MinimumQuantity = request.Quantity,
            DiscountRate = request.DiscountRate,
            Category = request.Category
        };

        await productRepository.AddAsync(product);

        var isDone = await unitOfWork.CompleteAsync();

        if (isDone > 0 && product.Id != Guid.Empty)
            return ProductDto.Create(product);
        else
            return Result.Failure<ProductDto>(new("Failed to add product, Database Error."));
    }
}
