using Common.CleanArch;
namespace Project.Application.Features.Users.Update;

public record UpdateUserCommand(long UserId, string FullName, string UserName, long RolID) : IResultRequest<bool>;