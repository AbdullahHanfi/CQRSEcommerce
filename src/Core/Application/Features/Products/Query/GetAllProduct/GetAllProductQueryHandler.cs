using Application.Abstractions.Messaging;
using Application.DTOs.User;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared.Collections;

namespace Application.Features.Products.Query.GetAllProduct;

public class GetAllProductQueryHandler(IRepository<Product> repository) : IQueryHandler<GetAllProductQuery, PaginatedList<ProductDto>>
{
    public async Task<Result<PaginatedList<ProductDto>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        //var products = repository.Skip((request.page - 1) * 10).Take(10).ToList();
        var products = await repository.GetAllAsync();

        var result = new PaginatedList<ProductDto>(ProductDto.Create([.. products]), await repository.CountAsync(), request.page, 10);

        return result;
    }
}
