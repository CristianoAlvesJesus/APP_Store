using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos;
using Store.Domain.Entities;
using Store.Domain.Extensions;
using Store.Domain.Repositories.Transaction;

namespace Store.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionWriteOnlyRepository, ITransactionReadOnlyRepository
{
    private readonly StoreDbContext _storeDbContext;
    public TransactionRepository(StoreDbContext storeDbContext) => _storeDbContext = storeDbContext;

    public async Task Add(Transaction transaction) => await _storeDbContext.Transactions.AddAsync(transaction);

    public async Task<IList<Transaction>> Filter(User user, FilterTransactionsDto filters)
    {
        var query = _storeDbContext.Transactions.AsNoTracking()
            .Where(_ => _.UserId == user.Id
            && _.Active);
        if(filters.TransactionType != 0)
        {
            query = query.Where(
                _ => _.TransactionType == filters.TransactionType);
        }
        if (filters.Description.NotEmpty())
        {
            query = query.Where(
                _ => _.Description!.Contains(filters.Description));
        }
        if (filters.Amount > 0)
        {
            query = query.Where(
                _ => _.Amount == filters.Amount);
        }
        return await query.ToListAsync();
    }

    public async Task<Transaction?> GetTransationById(User user, long transactionId)
    {
        return await _storeDbContext
             .Transactions
             .AsNoTracking()
             .FirstOrDefaultAsync(t => t.Active 
                                    && t.Id == transactionId 
                                    && t.UserId == user.Id);
    }
}
