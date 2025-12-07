using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.Domain.Entities;
using Project.Domain.Repositories;
using System.Data;

namespace Project.Infrastructure.Repositories;

public class UserRepository(IConfiguration configuration) : IUserRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException(nameof(configuration));

    // 1. Login: Buscar usuario por nombre
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = """
            SELECT UserID, UserFullName, UserName, PasswordHash, UserRolID 
            FROM Users 
            WHERE UserName = @UserName
        """;
        
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { UserName = userName });
    }

    // 2. Crear: Insertar y devolver ID
    public async Task<long> CreateAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = """
            INSERT INTO Users (UserFullName, UserName, PasswordHash, UserRolID)
            OUTPUT INSERTED.UserID
            VALUES (@UserFullName, @UserName, @PasswordHash, @UserRolID)
        """;

        return await connection.ExecuteScalarAsync<long>(query, user);
    }

    // 3. Listar: Traer todos
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Users";
        return await connection.QueryAsync<User>(query);
    }

    // 4. Borrar: Eliminar por ID
    public async Task<bool> DeleteAsync(long userId)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "DELETE FROM Users WHERE UserID = @UserID";
        var rows = await connection.ExecuteAsync(query, new { UserID = userId });
        return rows > 0;
    }
}