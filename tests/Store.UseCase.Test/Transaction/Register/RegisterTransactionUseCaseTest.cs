using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using Store.Application.UseCases.Transaction.Register;
using Store.Exceptions.ExceptionBase;
using Store.Exceptions;
using Store.UseCase.Test.Transaction.InlineDatas;

namespace Store.UseCase.Test.Transaction.Register;

public class RegisterTransactionUseCaseTest
{
    [Fact]
    public async Task Register_Transaction_Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRegisterTransactionFormDataBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Description);
        Assert.Equal(result.Description, request.Description);
    }


    //[Theory]
    //[ClassData(typeof(ImagesTypesInlineData))]
    //public async Task Register_Success_With_File(IFormFile file)
    //{
    //    (var user, _) = UserBuilder.Build(); 

    //    var request = RequestRegisterTransactionFormDataBuilder.Build(file);

    //    var useCase = CreateUseCase(user); 

    //    var result = await useCase.Execute(request);

    //    Assert.NotNull(result);
    //    Assert.NotEmpty(result.Description);
    //    Assert.Equal(result.Description, request.Description);
    //}

    [Fact]
    public async Task Error_Descripton_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRegisterTransactionFormDataBuilder.Build();
        request.Description = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        //Assert
        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.NotNull(exception);
        Assert.IsType<ErrorOnValidationException>(exception);
        Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.TRANSACTION_DESCRIPTION_EMPTY));
    }
 

    private static RegisterTransactionUseCase CreateUseCase(Store.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = TransactionWriteOnlyRepositoryBuilder.Build();

        return new RegisterTransactionUseCase(loggedUser, repository, unitOfWork, mapper);
    }
}
