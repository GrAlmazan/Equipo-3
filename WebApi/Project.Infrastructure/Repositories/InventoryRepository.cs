// Ubicación: WebApi/Project.Infrastructure/Repositories/InventoryRepository.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.Domain.Repositories;
using System.Data;

namespace Project.Infrastructure.Repositories;

public class InventoryRepository(IConfiguration configuration) : IInventoryRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<int> AdjustStockAsync(long productId, int quantity, long userId, string reason)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Iniciamos la transacción (Todo o Nada)
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Insertar Historial (InventoryMovements)
            const string sqlMovement = """
                INSERT INTO InventoryMovements (ProductID, UserID, Quantity, Reason)
                VALUES (@Pid, @Uid, @Qty, @Reason);
            """;
            using (var cmd1 = new SqlCommand(sqlMovement, connection, transaction))
            {
                cmd1.Parameters.AddWithValue("@Pid", productId);
                cmd1.Parameters.AddWithValue("@Uid", userId);
                cmd1.Parameters.AddWithValue("@Qty", quantity);
                cmd1.Parameters.AddWithValue("@Reason", reason);
                await cmd1.ExecuteNonQueryAsync();
            }

            // 2. Actualizar o Insertar Stock (Tabla Inventory)
            // Si el producto ya está en inventario, actualiza. Si no, lo crea.
            const string sqlUpdateStock = """
                MERGE INTO Inventory AS Target
                USING (SELECT @Pid AS ProductID) AS Source
                ON (Target.ProductID = Source.ProductID)
                WHEN MATCHED THEN
                    UPDATE SET Stock = Stock + @Qty, LastUpdated = GETUTCDATE()
                WHEN NOT MATCHED THEN
                    INSERT (ProductID, Stock, LastUpdated) VALUES (@Pid, @Qty, GETUTCDATE());
            """;
            using (var cmd2 = new SqlCommand(sqlUpdateStock, connection, transaction))
            {
                cmd2.Parameters.AddWithValue("@Pid", productId);
                cmd2.Parameters.AddWithValue("@Qty", quantity);
                await cmd2.ExecuteNonQueryAsync();
            }

            // 3. Obtener el stock final para devolverlo
            int currentStock = 0;
            const string sqlGet = "SELECT Stock FROM Inventory WHERE ProductID = @Pid";
            using (var cmd3 = new SqlCommand(sqlGet, connection, transaction))
            {
                cmd3.Parameters.AddWithValue("@Pid", productId);
                var result = await cmd3.ExecuteScalarAsync();
                if (result != null) currentStock = (int)result;
            }

            // Confirmar transacción
            transaction.Commit();
            return currentStock;
        }
        catch
        {
            transaction.Rollback(); // Si falla algo, deshacer todo
            throw;
        }
    }

    public async Task<int> GetStockAsync(long productId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string query = "SELECT Stock FROM Inventory WHERE ProductID = @Pid";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Pid", productId);
        
        var result = await command.ExecuteScalarAsync();
        return result != null ? (int)result : 0;
    }
}