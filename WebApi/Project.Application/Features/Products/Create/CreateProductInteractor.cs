// Ubicación: WebApi/Project.Application/Features/Products/Create/CreateProductInteractor.cs
using Common;
using Common.CleanArch;
using Project.Domain.Entities;
using Project.Domain.Repositories;

namespace Project.Application.Features.Products.Create;

public class CreateProductInteractor(IProductRepository repository) : ResultInteractorBase<CreateProductCommand, long>
{
    public override async Task<Result<long>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Validaciones simples
        if (string.IsNullOrEmpty(request.Name)) return Fail("El nombre es obligatorio.");
        if (request.Price < 0) return Fail("El precio no puede ser negativo.");

        // 2. Crear la entidad
        var newProduct = new Product
        {
            Name = request.Name,
            SKU = request.SKU,
            Price = request.Price,
            IsActive = true
        };

        // 3. Guardar en BD
        var id = await repository.CreateAsync(newProduct);

        // 4. Retornar el ID creado
        return OK(id);
    }
}