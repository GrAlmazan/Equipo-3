// Ubicación: WebApi/Project.Domain/Repositories/IProductRepository.cs
using Project.Domain.Entities;

namespace Project.Domain.Repositories;

public interface IProductRepository
{
    Task<long> CreateAsync(Product product); // Devuelve el ID creado
    Task<IEnumerable<Product>> GetAllAsync(); // Lista todos
}