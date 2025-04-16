using Store.Communication.Enums;

namespace Store.Communication.Responses;

public class ResponseTransactionJson
{
    public string Id { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
}