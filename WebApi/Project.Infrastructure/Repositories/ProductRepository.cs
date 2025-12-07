using Dapper;
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

    // Crear Producto y devolver ID
    public async Task<long> CreateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = """
            INSERT INTO Products (Name, SKU, Price, IsActive)
            OUTPUT INSERTED.ProductID
            VALUES (@Name, @SKU, @Price, @IsActive)
        """;

        // Dapper mapea las propiedades del objeto 'product' a los parámetros @Name, @SKU, etc.
        return await connection.ExecuteScalarAsync<long>(query, product);
    }

    // Listar Productos
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT ProductID, Name, SKU, Price, IsActive FROM Products";
        return await connection.QueryAsync<Product>(query);
    }
}