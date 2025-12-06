// Ubicación: WebApi/Project.Domain/Repositories/IInventoryRepository.cs
namespace Project.Domain.Repositories;

public interface IInventoryRepository
{
    // Devuelve el nuevo stock actual después del movimiento
    Task<int> AdjustStockAsync(long productId, int quantity, long userId, string reason);
    
    // Consultar stock actual de un producto
    Task<int> GetStockAsync(long productId);
}