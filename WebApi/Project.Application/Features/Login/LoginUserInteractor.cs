// Ubicación: WebApi/Project.Application/Features/Login/LoginUserInteractor.cs
using System.Security.Cryptography;
using System.Text;
using Common;             // Para Result<T>
using Common.CleanArch;   // Para ResultInteractorBase
using Project.Domain.Repositories;

namespace Project.Application.Features.Login;

public class LoginUserInteractor(IUserRepository userRepository) : ResultInteractorBase<LoginUserCommand, string>
{
    // En .NET 8 podemos usar el "Primary Constructor" (arriba en la clase) 
    // para inyectar el repositorio directamente, ahorrando código.

    public override async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario en la BD
        var user = await userRepository.GetByUserNameAsync(request.UserName);
        
        // Validamos si no existe
        if (user is null)
        {
            return Fail("Credenciales inválidas."); 
        }

        // 2. Verificar Contraseña (Comparando el Hash)
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            return Fail("Credenciales inválidas.");
        }

        // 3. Retornar éxito
        // Nota: En un futuro aquí devolveremos el JWT real.
        return OK($"¡Bienvenido al sistema, {user.UserFullName}!");
    }

    private static bool VerifyPassword(string password, byte[] storedHash)
    {
        // Calculamos el hash de la contraseña que ingresó el usuario
        var computedHash = SHA256.HashData(Encoding.UTF8.GetBytes(password)); // Método optimizado en .NET modernos
        
        // Comparamos con el que está en la base de datos
        return computedHash.SequenceEqual(storedHash);
    }
}