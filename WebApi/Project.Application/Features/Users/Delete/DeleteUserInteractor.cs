using Common;
using Common.CleanArch;
using Project.Domain.Repositories;

namespace Project.Application.Features.Users.Delete;

public class DeleteUserInteractor(IUserRepository repository) : ResultInteractorBase<DeleteUserCommand, bool>
{
    public override async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var deleted = await repository.DeleteAsync(request.UserId);
        if (!deleted) return Fail("Usuario no encontrado o no se pudo eliminar.");

        return OK(true);
    }
}