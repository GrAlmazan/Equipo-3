// Ubicación: WebApi/Project.Application/Features/Inventory/AdjustStockCommand.cs
using Common.CleanArch;

namespace Project.Application.Features.Inventory;

// Recibe ID producto, Cantidad (+/-), ID Usuario y Razón. Devuelve el nuevo stock (int).
public record AdjustStockCommand(long ProductId, int Quantity, long UserId, string Reason) : IResultRequest<int>;