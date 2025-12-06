using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Products.Create;
using Project.Application.Features.Products.Get;
using Project.Domain.Entities;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    // 👇 CREAR: SOLO ADMIN (Candado Rojo)
    [Authorize(Roles = "Admin")] 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        if (result is Common.SuccessResult<long> success)
            return Ok(new { ProductID = success.Data, Message = "Producto creado exitosamente" });
        
        if (result is Common.FailureResult<long> failure)
            return BadRequest(new { Error = failure.Message });
        return BadRequest();
    }

    // 👇 VER LISTA: CUALQUIERA QUE TENGA LOGIN (Candado Azul)
    [Authorize] 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllProductsQuery());
        
        // CORRECCIÓN AQUÍ: Antes decía <IEnumerable<User>>, ahora dice <IEnumerable<Product>>
        if (result is Common.SuccessResult<IEnumerable<Product>> success) 
        {
            return Ok(success.Data);
        }

        return BadRequest();
    }
}