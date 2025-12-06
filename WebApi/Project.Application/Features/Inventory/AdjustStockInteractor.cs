using Common;
using Common.CleanArch;
using Project.Domain.Repositories;
using Prometheus; // 👈 Necesario para las métricas

namespace Project.Application.Features.Inventory;

public class AdjustStockInteractor(IInventoryRepository repository) : ResultInteractorBase<AdjustStockCommand, int>
{
    // 1. Definimos la métrica para Grafana (Un contador de movimientos)
    private static readonly Counter MovementCounter = Metrics
        .CreateCounter("inventory_movements_total", "Total de movimientos de inventario", new CounterConfiguration
        {
            LabelNames = ["type", "reason"] // Etiquetas para filtrar en Grafana
        });

    public override async Task<Result<int>> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        // Validaciones básicas
        if (request.Quantity == 0) return Fail("La cantidad no puede ser cero.");
        if (string.IsNullOrWhiteSpace(request.Reason)) return Fail("Debes especificar una razón.");

        // ==============================================================================
        // 2. REGLA DE NEGOCIO: Evitar Stock Negativo
        // ==============================================================================
        if (request.Quantity < 0) // Si es una salida (venta/resta)
        {
            // Consultamos cuánto hay ANTES de intentar restar
            var currentStock = await repository.GetStockAsync(request.ProductId);
            
            // Si lo que queremos restar es mayor a lo que hay... Error.
            if ((currentStock + request.Quantity) < 0)
            {
                return Fail($"Stock insuficiente. Tienes {currentStock} y quieres restar {Math.Abs(request.Quantity)}.");
            }
        }

        try 
        {
            // Ejecutar movimiento en BD
            var newStock = await repository.AdjustStockAsync(
                request.ProductId, 
                request.Quantity, 
                request.UserId, 
                request.Reason
            );

            // ==============================================================================
            // 3. METRICA: Registrar el evento para Grafana
            // ==============================================================================
            string type = request.Quantity > 0 ? "Entrada" : "Salida";
            MovementCounter.WithLabels(type, request.Reason).Inc(); // Incrementa el contador +1

            return OK(newStock);
        }
        catch (Exception ex)
        {
            return Fail($"Error crítico: {ex.Message}");
        }
    }
}