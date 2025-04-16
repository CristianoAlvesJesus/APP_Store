using Moq;
using Store.Domain.Dtos;
using Store.Domain.Entities;
using Store.Domain.Repositories.Transaction;

namespace CommonTestUtilities.Repositories;

public class TransactionReadOnlyRepositoryBuilder
{
    private readonly Mock<ITransactionReadOnlyRepository> _repository;

    public TransactionReadOnlyRepositoryBuilder() => _repository = new Mock<ITransactionReadOnlyRepository>();

    public TransactionReadOnlyRepositoryBuilder Filter(User user, IList<Transaction> transactions)
    {
        _repository.Setup(repository => repository.Filter(user, It.IsAny<FilterTransactionsDto>())).ReturnsAsync(transactions);

        return this;
    }
    public TransactionReadOnlyRepositoryBuilder GetTransationById(User user, Transaction? transaction)
    {
        if (transaction is not null)
            _repository.Setup(repository => repository.GetTransationById(user, transaction.Id)).ReturnsAsync(transaction);

        return this;
    }
    public ITransactionReadOnlyRepository Build() => _repository.Object;
}
