using Store.Domain.Entities;
using Store.Domain.Services.ServiceBus;

namespace Store.Infrastructure.Services.ServiceBus;

public class DeleteUserQueue : IDeleteUserQueue
{
    public Task SendMessage(User user)
    {
        throw new NotImplementedException();
    }
}
