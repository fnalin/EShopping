using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByNameQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var productlist = await _productRepository.GetProductByName(request.Name);
        return Mappers.ProductMapper.Mapper.Map<IList<ProductResponse>>(productlist);
    }
}