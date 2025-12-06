// Ubicación: WebApi/Project.Application/Features/Products/Get/GetAllProductsInteractor.cs
using Common;
using Common.CleanArch;
using Project.Domain.Entities;
using Project.Domain.Repositories;

namespace Project.Application.Features.Products.Get;

public class GetAllProductsInteractor(IProductRepository repository) 
    : ResultInteractorBase<GetAllProductsQuery, IEnumerable<Product>>
{
    public override async Task<Result<IEnumerable<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllAsync();
        return OK(list);
    }
}