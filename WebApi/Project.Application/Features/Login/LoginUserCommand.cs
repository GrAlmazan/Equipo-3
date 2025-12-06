// Ubicación: WebApi/Project.Application/Features/Login/LoginUserCommand.cs
using Common.CleanArch;

namespace Project.Application.Features.Login;

// Implementamos IResultRequest<string> porque devolveremos un mensaje o token (string)
public record LoginUserCommand(string UserName, string Password) : IResultRequest<string>;