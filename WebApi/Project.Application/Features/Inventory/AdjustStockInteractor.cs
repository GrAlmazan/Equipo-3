// Ubicación: WebApi/Project.Application/Features/Inventory/AdjustStockInteractor.cs
using Common;
using Common.CleanArch;
using Project.Domain.Repositories;

namespace Project.Application.Features.Inventory;

public class AdjustStockInteractor(IInventoryRepository repository) : ResultInteractorBase<AdjustStockCommand, int>
{
    public override async Task<Result<int>> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        // 1. Validaciones
        if (request.Quantity == 0) 
            return Fail("La cantidad no puede ser cero.");
        
        if (string.IsNullOrWhiteSpace(request.Reason)) 
            return Fail("Debes especificar una razón para el movimiento.");

        // 2. Ejecutar movimiento en BD
        try 
        {
            var newStock = await repository.AdjustStockAsync(
                request.ProductId, 
                request.Quantity, 
                request.UserId, 
                request.Reason
            );

            // 3. Retornar el stock final
            return OK(newStock);
        }
        catch (Exception ex)
        {
            // Loguear el error real aquí si tuvieras logger
            return Fail($"Error al ajustar inventario: {ex.Message}");
        }
    }
}