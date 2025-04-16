using Store.Domain.Entities;

namespace Store.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}
