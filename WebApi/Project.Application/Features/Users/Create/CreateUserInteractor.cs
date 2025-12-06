using System.Security.Cryptography;
using System.Text;
using Common;
using Common.CleanArch;
using Project.Domain.Entities;
using Project.Domain.Repositories;

namespace Project.Application.Features.Users.Create;

public class CreateUserInteractor(IUserRepository repository) : ResultInteractorBase<CreateUserCommand, long>
{
    public override async Task<Result<long>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validar si el usuario ya existe (Opcional, pero recomendado)
        var existing = await repository.GetByUserNameAsync(request.UserName);
        if (existing != null) return Fail($"El usuario '{request.UserName}' ya existe.");

        // Encriptar contraseña
        byte[] passwordHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.Password));

        var newUser = new User
        {
            UserFullName = request.FullName,
            UserName = request.UserName,
            PasswordHash = passwordHash,
            UserRolID = request.RolID
        };

        var id = await repository.CreateAsync(newUser);
        return OK(id);
    }
}