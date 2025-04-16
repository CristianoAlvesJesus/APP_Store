using FluentValidation;
using Store.Communication.Requests;
using Store.Exceptions;

namespace Store.Application.UseCases.Transaction.Filter;

public class FilterTransactionValidator : AbstractValidator<RequestFilterTransactionJson>
{
    public FilterTransactionValidator()
    {
        When(_ =>  _.TransactionType != 0, () =>
        {
            RuleFor(transaction => transaction.TransactionType).IsInEnum().WithMessage(ResourceMessageException.TRANSACTION_TYPE_NOT_SUPPORTED);
        }); 
    }
}