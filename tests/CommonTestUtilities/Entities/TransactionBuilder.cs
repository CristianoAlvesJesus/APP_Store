using Bogus;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class TransactionBuilder
{
    public static IList<Transaction> Collection(User user, uint count = 2)
    {
        var list = new List<Transaction>();

        if (count == 0)
            count = 1;

        var transactionId = 1;

        for (int i = 0; i < count; i++)
        {
            var fakeTransaction = Build(user);
            fakeTransaction.Id = transactionId++;

            list.Add(fakeTransaction);
        }

        return list;
    }

    public static Transaction Build(User user)
    {
        return new Faker<Transaction>()
            .RuleFor(t => t.Id, () => 1)
            .RuleFor(t => t.Description, (f) => f.Lorem.Word())
            .RuleFor(t => t.TransactionType, (f) => f.PickRandom<TransactionType>())
            .RuleFor(t => t.Amount, f => f.Finance.Amount())
            .RuleFor(t => t.UserId, _ => user.Id);
    }
}
