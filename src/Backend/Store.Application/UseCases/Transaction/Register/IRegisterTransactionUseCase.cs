using Store.Communication.Requests;
using Store.Communication.Responses;

namespace Store.Application.UseCases.Transaction.Register;

public interface IRegisterTransactionUseCase
{
    Task<ResponseRegiteredTransactionJson> Execute(RequestRegisterTransactionFormData request);
}
