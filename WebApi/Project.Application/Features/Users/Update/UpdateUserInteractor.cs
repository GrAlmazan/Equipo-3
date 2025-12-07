using Common;
using Common.CleanArch;
using Project.Domain.Repositories;

namespace Project.Application.Features.Users.Update;

public class UpdateUserInteractor(IUserRepository repository) : ResultInteractorBase<UpdateUserCommand, bool>
{
    public override async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updated = await repository.UpdateAsync(request.UserId, request.FullName, request.UserName, request.RolID);
        
        if (!updated) return Fail("No se pudo actualizar el usuario o no existe.");
        return OK(true);
    }
}