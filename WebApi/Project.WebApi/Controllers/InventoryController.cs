// Ubicación: WebApi/Project.WebApi/Controllers/InventoryController.cs
using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Inventory;
using Microsoft.AspNetCore.Authorization;

namespace Project.WebApi.Controllers;

[Authorize] // <--- ¡ESTO ES EL CANDADO! 🔒
[ApiController]
[Route("api/[controller]")]
public class InventoryController(IMediator mediator) : ControllerBase
{
    // POST api/inventory/adjust
    [HttpPost("adjust")]
    public async Task<IActionResult> AdjustStock([FromBody] AdjustStockCommand command)
    {
        var result = await mediator.Send(command);

        if (result is Common.SuccessResult<int> success)
        {
            return Ok(new { 
                NewStock = success.Data, 
                Message = "Inventario actualizado correctamente." 
            });
        }
        
        if (result is Common.FailureResult<int> failure)
        {
            return BadRequest(new { Error = failure.Message });
        }

        return BadRequest();
    }
}