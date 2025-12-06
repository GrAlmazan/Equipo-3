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

    // 1. Obtener por Usuario (Para el Login)
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "SELECT UserID, UserFullName, UserName, PasswordHash, UserRolID FROM Users WHERE UserName = @UserName";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserName", userName);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapUser(reader);
        }
        return null; 
    }

    // 2. Crear Usuario (INSERT)
    public async Task<long> CreateAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = """
            INSERT INTO Users (UserFullName, UserName, PasswordHash, UserRolID)
            OUTPUT INSERTED.UserID
            VALUES (@FullName, @UserName, @Hash, @RolID)
        """;

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@FullName", user.UserFullName);
        command.Parameters.AddWithValue("@UserName", user.UserName);
        command.Parameters.AddWithValue("@Hash", user.PasswordHash);
        command.Parameters.AddWithValue("@RolID", user.UserRolID);

        return (long)await command.ExecuteScalarAsync()!;
    }

    // 3. Listar Todos (SELECT)
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var list = new List<User>();
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "SELECT UserID, UserFullName, UserName, PasswordHash, UserRolID FROM Users";
        using var command = new SqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            list.Add(MapUser(reader));
        }
        return list;
    }

    // 4. Borrar Usuario (DELETE)
    public async Task<bool> DeleteAsync(long userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "DELETE FROM Users WHERE UserID = @UserID";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", userId);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    // Helper para no repetir código de mapeo
    private static User MapUser(SqlDataReader reader)
    {
        return new User
        {
            UserID = (long)reader["UserID"],
            UserFullName = (string)reader["UserFullName"],
            UserName = (string)reader["UserName"],
            PasswordHash = (byte[])reader["PasswordHash"],
            UserRolID = (long)reader["UserRolID"]
        };
    }
}