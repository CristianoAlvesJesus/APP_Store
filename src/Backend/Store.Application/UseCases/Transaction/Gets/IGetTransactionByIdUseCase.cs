using Store.Communication.Responses;

namespace Store.Application.UseCases.Transaction.Gets;

public interface IGetTransactionByIdUseCase
{
    Task<ResponseTransactionJson> Execute(long transactionId);
}
