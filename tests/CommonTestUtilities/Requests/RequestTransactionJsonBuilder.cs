using Bogus;
using Store.Communication.Requests;
using Store.Communication.Enums;

namespace CommonTestUtilities.Requests;

public class RequestTransactionJsonBuilder
{
    public static RequestTransactionJson Build()
    {
        return new Faker<RequestTransactionJson>()
            .RuleFor(t => t.Description, f => f.Commerce.ProductName())
            .RuleFor(t => t.Amount, f => f.Finance.Amount())
            .RuleFor(t => t.TransactionType, f => f.PickRandom<TransactionType>());
    }
}