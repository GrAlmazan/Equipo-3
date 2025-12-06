using Common.CleanArch;
namespace Project.Application.Features.Users.Delete;

public record DeleteUserCommand(long UserId) : IResultRequest<bool>;