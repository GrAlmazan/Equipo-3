// Ubicación: WebApi/Project.Domain/Entities/Product.cs
namespace Project.Domain.Entities;

public class Product
{
    public long ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty; // Código único
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
}