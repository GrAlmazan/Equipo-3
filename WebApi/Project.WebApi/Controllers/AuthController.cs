using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Login;
using Project.Application.Features.Users.Create; // Necesario para crear usuario

namespace Project.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    // 1. LOGIN (Ya lo tenías)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result is Common.SuccessResult<string> success)
            return Ok(new { Token = success.Data });
        
        if (result is Common.FailureResult<string> failure)
            return Unauthorized(new { Error = failure.Message });

        return BadRequest();
    }

    // 2. REGISTRO PÚBLICO (Nuevo)
    // No tiene [Authorize], así que cualquiera puede usarlo.
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // AQUÍ ESTÁ EL TRUCO: 👇
        // Recibimos los datos básicos, pero nosotros FORZAMOS el RolID = 2
        var command = new CreateUserCommand(
            request.FullName, 
            request.UserName, 
            request.Password, 
            2 // <--- Rol 2 (User) Hardcodeado. ¡Nadie puede registrarse como Admin aquí!
        );

        var result = await mediator.Send(command);
        
        if (result is Common.SuccessResult<long> success) 
            return Ok(new { UserId = success.Data, Message = "Te has registrado exitosamente. Ahora puedes hacer Login." });

        if (result is Common.FailureResult<long> failure) 
            return BadRequest(new { Error = failure.Message });

        return BadRequest();
    }
}

// Clase auxiliar para recibir solo los datos necesarios en el registro
public record RegisterRequest(string FullName, string UserName, string Password);