using Store.Domain.Dtos;

namespace Store.Domain.Repositories.Transaction;

public interface ITransactionReadOnlyRepository
{
    Task<IList<Entities.Transaction>> Filter(Entities.User user, FilterTransactionsDto filters);
    Task<Entities.Transaction?> GetTransationById(Entities.User user, long transactionId);
}
