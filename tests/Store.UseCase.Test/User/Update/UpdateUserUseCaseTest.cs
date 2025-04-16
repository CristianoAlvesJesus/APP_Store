using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Store.Application.UseCases.User.Update;
using Store.Domain.Extensions;
using Store.Exceptions.ExceptionBase;
using Store.Exceptions;

namespace Store.UseCase.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);
         

        var exception = await Record.ExceptionAsync(act);
        Assert.Null(exception);

        Assert.Equal(user.Name,request.Name);
        Assert.Equal(user.Email, request.Email); 
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); }; 

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act) ;
        Assert.NotNull(exception);
        Assert.IsType<ErrorOnValidationException>(exception);
        Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.NAME_EMPTY)); 
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Email);

        Func<Task> act = async () => { await useCase.Execute(request); };

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.NotNull(exception);
        Assert.IsType<ErrorOnValidationException>(exception);
        Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.EMAIL_ALREADY_REGISTERED));
    }
    private static UpdateUserUseCase CreateUseCase(Store.Domain.Entities.User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        if (email.NotEmpty())
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(loggedUser, userUpdateRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
    }
}
