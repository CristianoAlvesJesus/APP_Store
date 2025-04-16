using Store.Communication.Enums;

namespace Store.Communication.Requests
{
    public class RequestFilterTransactionJson
    {
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
