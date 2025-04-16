using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Store.Application.UseCases.User.ChangePassword;
using Store.Communication.Requests;
using Store.Exceptions.ExceptionBase;
using Store.Exceptions;

namespace Store.UseCase.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await Record.ExceptionAsync(act);
        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            CurrentPassword = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.NotNull(exception);
        Assert.IsType<ErrorOnValidationException>(exception);
        Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.PASSWORD_EMPTY)); 
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.NotNull(exception);
        Assert.IsType<ErrorOnValidationException>(exception);
        Assert.True(exception.GetErrorMessages().Contains(ResourceMessageException.PASSWORD_DIFFERENT_CURRENT_PASSWORD)); 
    }

    private static ChangePasswordUseCase CreateUseCase(Store.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncripter, userUpdateRepository, unitOfWork);
    }
}