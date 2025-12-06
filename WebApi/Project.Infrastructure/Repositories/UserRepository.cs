// Ubicación: WebApi/Project.Infrastructure/Repositories/UserRepository.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.Domain.Entities;
using Project.Domain.Repositories;
using System.Data;

namespace Project.Infrastructure.Repositories;

public class UserRepository(IConfiguration configuration) : IUserRepository
{
    // Usamos Primary Constructor de nuevo para inyectar IConfiguration
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Consulta SQL directa y eficiente
        const string query = """
            SELECT UserID, UserFullName, UserName, PasswordHash, UserRolID 
            FROM Users 
            WHERE UserName = @UserName
        """;
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserName", userName);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
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

        return null; 
    }
}