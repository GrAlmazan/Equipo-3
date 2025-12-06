using Common;
using Common.CleanArch;
using Project.Domain.Entities;
using Project.Domain.Repositories;

namespace Project.Application.Features.Users.Get;

public class GetUsersInteractor(IUserRepository repository) : ResultInteractorBase<GetUsersQuery, IEnumerable<User>>
{
    public override async Task<Result<IEnumerable<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await repository.GetAllAsync();
        // Por seguridad, limpiamos el hash antes de enviar la lista al frontend
        foreach(var u in users) u.PasswordHash = []; 
        return OK(users);
    }
}