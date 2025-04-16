using FluentValidation;
using Store.Communication.Requests;
using Store.Exceptions; 

namespace Store.Application.UseCases.Transaction;

public class TransactionValidator : AbstractValidator<RequestTransactionJson>
{
 
    public TransactionValidator()
    {
        RuleFor(transaction => transaction.Description).NotEmpty().WithMessage(ResourceMessageException.TRANSACTION_DESCRIPTION_EMPTY); 
        RuleFor(transaction => transaction.TransactionType).IsInEnum().WithMessage(ResourceMessageException.TRANSACTION_TYPE_NOT_SUPPORTED); 
        RuleFor(transaction => transaction.Amount)
            .NotEmpty().WithMessage(ResourceMessageException.AMOUNT_NOT_EMPTY)
            .GreaterThan(0).WithMessage(ResourceMessageException.AMOUNT_GREATER_THAN_ZERO)
            .ScalePrecision(2, 10).WithMessage(ResourceMessageException.AMOUNT_MAXIMUM_TWO_UP_TO_TEN_DIGITS); 

    } 
}
