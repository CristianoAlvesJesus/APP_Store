using Store.Domain.Entities;

namespace Store.Domain.Services.ServiceBus;

public interface IDeleteUserQueue
{
    Task SendMessage(User user);
}
