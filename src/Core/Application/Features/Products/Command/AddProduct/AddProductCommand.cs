namespace Application.Features.Products.Command.AddProduct;
using Application.Abstractions.Messaging;
using Application.DTOs.User;

public record AddProductCommand(string ProductCode,string Name, Stream ImageStream,string ContentType,decimal Price,int Quantity,decimal DiscountRate,string Category) : ICommand<ProductDto>;
