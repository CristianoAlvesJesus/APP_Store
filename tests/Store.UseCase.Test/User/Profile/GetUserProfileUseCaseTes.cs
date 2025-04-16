using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using Store.Application.UseCases.User.Profile;

namespace Store.UseCase.Test.User.Profile;

public class GetUserProfileUseCaseTes
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        Assert.NotNull(result);
        Assert.True(result.Name == user.Name);
        Assert.True(result.Email == user.Email);
    }

    private static GetUserProfileUseCase CreateUseCase(Store.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserProfileUseCase(loggedUser, mapper);
    }
}
