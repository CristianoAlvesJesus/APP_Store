using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using Store.Exceptions.ExceptionBase;
using Store.Exceptions;
using Store.Application.UseCases.Transaction.Gets;
using CommonTestUtilities.Repositories;

namespace Store.WebAPI.Test.Transaction.Gets;

public class GetTransactionByIdUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var transaction = TransactionBuilder.Build(user);

        var useCase = CreateUseCase(user, transaction);

        var result = await useCase.Execute(transaction.Id);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(result.Description, transaction.Description);
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(transactionId: 10); };

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.NotNull(exception);
        Assert.IsType<NotFoundException>(exception);
        Assert.Contains(ResourceMessageException.TRANSACTION_NOT_FOUND, exception.Message);

    }

    private static GetTransactionByIdUseCase CreateUseCase(
        Store.Domain.Entities.User user,
        Domain.Entities.Transaction? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new TransactionReadOnlyRepositoryBuilder().GetTransationById(user, recipe).Build();

        return new GetTransactionByIdUseCase(mapper, loggedUser, repository);
    }
}
