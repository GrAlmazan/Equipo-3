// Ubicación: WebApi/Project.Application/Features/Products/Create/CreateProductCommand.cs
using Common.CleanArch;

namespace Project.Application.Features.Products.Create;

// Recibimos Nombre, SKU y Precio. Devolvemos el ID del producto creado (long).
public record CreateProductCommand(string Name, string SKU, decimal Price) : IResultRequest<long>;