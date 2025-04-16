﻿using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Store.Application.UseCases.Login.DoLogin;
using Store.Exceptions;
using Store.Exceptions.ExceptionBase;

namespace Store.UseCase.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user , var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new Communication.Requests.RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Name);
        Assert.Equal(result.Name, user.Name); 
        Assert.NotNull(result.Tokens);
        Assert.NotEmpty(result.Tokens.AccessToken);
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(request); };
        var exception = await Assert.ThrowsAsync<InvalidLoginException>(act);
        Assert.NotNull(exception);
        Assert.IsType<InvalidLoginException>(exception);
        Assert.Contains(exception.Message, ResourceMessageException.EMAIL_OR_PASSWORD_INVALID);

    }
    private static DoLoginUseCase CreateUseCase(Store.Domain.Entities.User? user = null )
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), 
                                  passwordEncripter, 
                                  accessTokenGenerator,
                                  unitOfWork, 
                                  refreshTokenGenerator, 
                                  tokenRepository);
    }
}