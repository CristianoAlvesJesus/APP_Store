using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.Application.UseCases.Transaction.Filter;

public interface IFilterTransactionUseCase
{
      Task<ResponseTransactionsJson> Execute(RequestFilterTransactionJson request);
}
