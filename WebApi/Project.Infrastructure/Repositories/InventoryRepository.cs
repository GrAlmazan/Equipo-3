using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.Domain.Repositories;
using System.Data;

namespace Project.Infrastructure.Repositories;

public class InventoryRepository(IConfiguration configuration) : IInventoryRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException(nameof(configuration));

    // Ajustar Stock (Transacción compleja)
    public async Task<int> AdjustStockAsync(long productId, int quantity, long userId, string reason)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Iniciamos transacción
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Insertar Historial
            const string sqlMovement = """
                INSERT INTO InventoryMovements (ProductID, UserID, Quantity, Reason)
                VALUES (@Pid, @Uid, @Qty, @Reason);
            """;
            await connection.ExecuteAsync(sqlMovement, 
                new { Pid = productId, Uid = userId, Qty = quantity, Reason = reason }, 
                transaction);

            // 2. Actualizar o Insertar Stock (MERGE)
            const string sqlUpdateStock = """
                MERGE INTO Inventory AS Target
                USING (SELECT @Pid AS ProductID) AS Source
                ON (Target.ProductID = Source.ProductID)
                WHEN MATCHED THEN
                    UPDATE SET Stock = Stock + @Qty, LastUpdated = GETUTCDATE()
                WHEN NOT MATCHED THEN
                    INSERT (ProductID, Stock, LastUpdated) VALUES (@Pid, @Qty, GETUTCDATE());
            """;
            await connection.ExecuteAsync(sqlUpdateStock, 
                new { Pid = productId, Qty = quantity }, 
                transaction);

            // 3. Obtener stock final
            const string sqlGet = "SELECT Stock FROM Inventory WHERE ProductID = @Pid";
            var currentStock = await connection.ExecuteScalarAsync<int>(sqlGet, 
                new { Pid = productId }, 
                transaction);

            // Confirmar
            transaction.Commit();
            return currentStock;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // Consultar Stock
    public async Task<int> GetStockAsync(long productId)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT Stock FROM Inventory WHERE ProductID = @Pid";
        
        // ExecuteScalar devuelve null si no existe, usamos null-coalescing (?? 0) para devolver 0 en ese caso
        var result = await connection.ExecuteScalarAsync<int?>(query, new { Pid = productId });
        return result ?? 0;
    }
}