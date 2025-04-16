using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Transaction: EntityBase
{
    public string? Description { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public long UserId { get; set; }
}