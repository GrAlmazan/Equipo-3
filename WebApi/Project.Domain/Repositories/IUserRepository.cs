using Project.Domain.Entities;

namespace Project.Domain.Repositories;

public interface IUserRepository
{
    // Login
    Task<User?> GetByUserNameAsync(string userName);

    // CRUD Nuevos
    Task<long> CreateAsync(User user);      // Guardar usuario nuevo
    Task<bool> DeleteAsync(long userId);    // Borrar por ID
    Task<IEnumerable<User>> GetAllAsync();  // Ver lista de todos
}