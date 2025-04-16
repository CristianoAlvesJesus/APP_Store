using AutoMapper;
using Store.Communication.Responses;
using Store.Domain.Entities;

namespace Store.Application.Extensions;

public static class TransactionListExtension
{
    public static async Task<IList<ResponseShortTransactionJson>> MapToShortTransactionsJson(
           this IList<Transaction> transactions,
           User user, 
           IMapper mapper)
    {
        var result = transactions.Select(async transaction =>
        {
            var response = mapper.Map<ResponseShortTransactionJson>(transaction); 

            return response;
        });

        var response = await Task.WhenAll(result);

        return response;
    }
}

