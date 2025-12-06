using Common.CleanArch;
using Project.Domain.Entities;
namespace Project.Application.Features.Users.Get;

public record GetUsersQuery() : IResultRequest<IEnumerable<User>>;