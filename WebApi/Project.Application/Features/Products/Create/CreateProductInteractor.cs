using Common;
using Common.CleanArch;
using Project.Domain.Entities;
using Project.Domain.Repositories;

namespace Project.Application.Features.Products.Create;

public class CreateProductInteractor(IProductRepository repository) : ResultInteractorBase<CreateProductCommand, long>
{
    public override async Task<Result<long>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Si el código llega a este punto, FluentValidation ya garantizó que los datos son correctos.

        // 1. Crear la entidad
        var newProduct = new Product
        {
            Name = request.Name,
            SKU = request.SKU,
            Price = request.Price,
            IsActive = true
        };

        // 2. Guardar en BD
        var id = await repository.CreateAsync(newProduct);

        // 3. Retornar el ID creado
        return OK(id);
    }
}