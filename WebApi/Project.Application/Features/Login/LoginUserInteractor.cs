using System.Security.Cryptography;
using System.Text;
using Common;
using Common.CleanArch;
using Project.Application.Interfaces; // <--- Importante
using Project.Domain.Repositories;

namespace Project.Application.Features.Login;

public class LoginUserInteractor(IUserRepository userRepository, IJwtGenerator jwtGenerator) 
    : ResultInteractorBase<LoginUserCommand, string>
{
    public override async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario
        var user = await userRepository.GetByUserNameAsync(request.UserName);
        if (user is null) return Fail("Credenciales inválidas.");

        // 2. Verificar password
        if (!VerifyPassword(request.Password, user.PasswordHash)) return Fail("Credenciales inválidas.");

        // 3. GENERAR TOKEN REAL (Esto es lo que te falta)
        string roleName = user.UserRolID == 1 ? "Admin" : "User";
        
        // Esta línea convierte el usuario en un código secreto largo
        var token = jwtGenerator.GenerateToken(user.UserID, user.UserName, roleName);

        // Devolvemos el token cifrado
        return OK(token);
    }

    private static bool VerifyPassword(string password, byte[] storedHash)
    {
        var computedHash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}