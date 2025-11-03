
using Application.Abstractions.Messaging;
using Application.DTOs.User;
using Domain.Shared.Collections;

namespace Application.Features.Products.Query.GetAllProduct;

public record GetAllProductQuery(int page) : IQuery<PaginatedList<ProductDto>>;
