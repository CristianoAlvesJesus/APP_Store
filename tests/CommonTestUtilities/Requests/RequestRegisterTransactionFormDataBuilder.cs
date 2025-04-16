using Bogus;
using Microsoft.AspNetCore.Http;
using Store.Communication.Enums;
using Store.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterTransactionFormDataBuilder
{
    public static RequestRegisterTransactionFormData Build(IFormFile? formFile = null)
    {
        var step = 1;

        return new Faker<RequestRegisterTransactionFormData>()
            .RuleFor(t => t.Description, f => f.Lorem.Word())
            .RuleFor(t => t.Amount, f => f.Finance.Amount())
            .RuleFor(t => t.TransactionType, f => f.PickRandom<TransactionType>()); 
    }
}
