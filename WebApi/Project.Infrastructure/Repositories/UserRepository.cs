using Project.Domain.Entities;
using Project.Domain.Repositories;
using Project.Infrastructure.Generic;

namespace Project.Infrastructure.Repositories;

public class UserRepository(IGenericDB db) : IUserRepository
{
    // 1. Login
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        const string query = "SELECT * FROM Users WHERE UserName = @UserName";
        return await db.QueryFirstOrDefaultAsync<User>(query, new { UserName = userName });
    }

    // 2. Crear
    public async Task<long> CreateAsync(User user)
    {
        const string query = """
            INSERT INTO Users (UserFullName, UserName, PasswordHash, UserRolID)
            OUTPUT INSERTED.UserID
            VALUES (@UserFullName, @UserName, @PasswordHash, @UserRolID)
        """;
        return await db.ExecuteScalarAsync<long>(query, user);
    }

    // 3. Listar Todos
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await db.QueryAsync<User>("SELECT * FROM Users");
    }

    // 4. Borrar
    public async Task<bool> DeleteAsync(long userId)
    {
        var rows = await db.ExecuteAsync("DELETE FROM Users WHERE UserID = @UserID", new { UserID = userId });
        return rows > 0;
    }

    // 5. ACTUALIZAR (Nuevo requisito del checklist)
    public async Task<bool> UpdateAsync(long id, string fullName, string userName, long rolId)
    {
        const string query = @"
            UPDATE Users 
            SET UserFullName = @FullName, UserName = @UserName, UserRolID = @RolID
            WHERE UserID = @ID";
            
        var rows = await db.ExecuteAsync(query, new { ID = id, FullName = fullName, UserName = userName, RolID = rolId });
        return rows > 0;
    }
}