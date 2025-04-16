using Store.Domain.Enums;

namespace Store.Domain.Dtos;

public record FilterTransactionsDto
{
    public string? Description { get; init; }
    public TransactionType TransactionType { get; init; }
    public decimal Amount { get; init; }
}