// Ubicación: WebApi/Project.Infrastructure/Repositories/ProductRepository.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.Domain.Entities;
using Project.Domain.Repositories;
using System.Data;

namespace Project.Infrastructure.Repositories;

public class ProductRepository(IConfiguration configuration) : IProductRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<long> CreateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = """
            INSERT INTO Products (Name, SKU, Price, IsActive)
            OUTPUT INSERTED.ProductID
            VALUES (@Name, @SKU, @Price, @IsActive)
        """;

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@SKU", product.SKU);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@IsActive", product.IsActive);

        // Ejecuta y devuelve el ID del nuevo producto
        return (long)await command.ExecuteScalarAsync()!;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var list = new List<Product>();
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "SELECT ProductID, Name, SKU, Price, IsActive FROM Products";
        using var command = new SqlCommand(query, connection);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new Product
            {
                ProductID = (long)reader["ProductID"],
                Name = (string)reader["Name"],
                SKU = (string)reader["SKU"],
                Price = (decimal)reader["Price"],
                IsActive = (bool)reader["IsActive"]
            });
        }
        return list;
    }
}