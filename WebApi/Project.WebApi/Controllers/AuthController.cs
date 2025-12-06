using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Login;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        // Envía el comando al Interactor (la lógica que escribiste antes)
        var result = await mediator.Send(command);

        // Si fue exitoso (OK)
        if (result is Common.SuccessResult<string> success)
        {
            return Ok(new { Token = success.Data });
        }
        
        // Si falló (Fail)
        if (result is Common.FailureResult<string> failure)
        {
            return Unauthorized(new { Error = failure.Message });
        }

        return BadRequest();
    }
}