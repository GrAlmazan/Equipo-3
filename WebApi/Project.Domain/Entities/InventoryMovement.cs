// Ubicación: WebApi/Project.Domain/Entities/InventoryMovement.cs
namespace Project.Domain.Entities;

public class InventoryMovement
{
    public long MovementID { get; set; }
    public long ProductID { get; set; }
    public long UserID { get; set; }
    public int Quantity { get; set; } // Positivo (Entrada) o Negativo (Salida)
    public string Reason { get; set; } = string.Empty;
    public DateTime MovementDate { get; set; }
}