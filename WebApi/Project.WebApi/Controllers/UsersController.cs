using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Users.Create;
using Project.Application.Features.Users.Delete;
using Project.Application.Features.Users.Get;
using Project.Domain.Entities;

namespace Project.WebApi.Controllers;

[Authorize(Roles = "Admin")] // <--- ¡SOLO EL ADMIN PUEDE ENTRAR AQUÍ!
[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    // GET api/users (Ver lista)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetUsersQuery());
        if (result is Common.SuccessResult<IEnumerable<User>> success) return Ok(success.Data);
        return BadRequest();
    }

    // POST api/users (Crear nuevo)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        
        if (result is Common.SuccessResult<long> success) 
            return Ok(new { UserId = success.Data, Message = "Usuario creado exitosamente" });

        if (result is Common.FailureResult<long> failure) 
            return BadRequest(new { Error = failure.Message });

        return BadRequest();
    }

    // DELETE api/users/5 (Borrar)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(new DeleteUserCommand(id));

        if (result is Common.SuccessResult<bool>) 
            return Ok(new { Message = "Usuario eliminado." });

        if (result is Common.FailureResult<bool> failure) 
            return NotFound(new { Error = failure.Message });

        return BadRequest();
    }
}