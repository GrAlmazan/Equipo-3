// Ubicación: WebApi/Project.Application/Interfaces/IJwtGenerator.cs
namespace Project.Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(long userId, string userName, string role);
}