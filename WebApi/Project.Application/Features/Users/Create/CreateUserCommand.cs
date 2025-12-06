using Common.CleanArch;
namespace Project.Application.Features.Users.Create;

// Pedimos nombre, usuario, contraseña (texto) y rol
public record CreateUserCommand(string FullName, string UserName, string Password, long RolID) : IResultRequest<long>;