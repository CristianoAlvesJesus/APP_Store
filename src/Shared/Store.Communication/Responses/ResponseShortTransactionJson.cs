using Store.Communication.Enums;

namespace Store.Communication.Responses;

public class ResponseShortTransactionJson
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
}
