using Store.Communication.Enums;

namespace Store.Communication.Requests;

public class RequestTransactionJson
{
    public string Description { get; set; } = string.Empty;
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
}