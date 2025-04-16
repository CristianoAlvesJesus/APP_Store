using Bogus;
using Store.Communication.Enums;
using Store.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestFilterTransactionJsonBuilder
{
    public static RequestFilterTransactionJson Build()
    {
        return new Faker<RequestFilterTransactionJson>()
            .RuleFor(_ => _.Description, f => f.Lorem.Word())
            .RuleFor(_ => _.TransactionType, f => f.PickRandom<TransactionType>())
            .RuleFor(t => t.Amount, f => f.Finance.Amount()); 
    }
}
