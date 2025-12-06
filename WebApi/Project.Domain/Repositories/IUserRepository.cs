// Ubicación: WebApi/Project.Domain/Repositories/IUserRepository.cs
using Project.Domain.Entities;

namespace Project.Domain.Repositories;

public interface IUserRepository
{
    // Usamos Task porque la operación será asíncrona
    Task<User?> GetByUserNameAsync(string userName);
}