using Moq;
using Project.Application.Features.Users.Create;
using Project.Domain.Entities;
using Project.Domain.Repositories;
using Xunit;

namespace Project.Tests;

public class CreateUserInteractorTests
{
    [Fact]
    public async Task Handle_Should_Return_Success_When_User_Is_Created()
    {
        // ARRANGE (Preparar)
        var mockRepo = new Mock<IUserRepository>();
        
        // Simulamos que el usuario NO existe previamente
        mockRepo.Setup(r => r.GetByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

        // Simulamos que al crear devuelve el ID 10
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(10);

        var interactor = new CreateUserInteractor(mockRepo.Object);
        var command = new CreateUserCommand("Juan Perez", "juanp", "123456", 2);

        // ACT (Actuar)
        var result = await interactor.Handle(command, CancellationToken.None);

        // ASSERT (Verificar)
        // Verificamos que sea un SuccessResult
        Assert.IsType<Common.SuccessResult<long>>(result);
        
        // Verificamos que el dato devuelto sea 10
        var successResult = (Common.SuccessResult<long>)result;
        Assert.Equal(10, successResult.Data);
    }
}