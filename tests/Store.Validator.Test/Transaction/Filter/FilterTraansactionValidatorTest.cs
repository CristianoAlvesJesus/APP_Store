using CommonTestUtilities.Requests;
using Store.Application.UseCases.Transaction.Filter;
using Store.Exceptions;

namespace Store.Validator.Test.Transaction.Filter;

public class FilterTraansactionValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new FilterTransactionValidator();

        var request = RequestFilterTransactionJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Invalid_TransactionType_TRANSACTION_TYPE_NOT_SUPPORTED()
    {
        var validator = new FilterTransactionValidator();

        var request = RequestFilterTransactionJsonBuilder.Build();
        request.TransactionType = (Store.Communication.Enums.TransactionType)10;

        var result = validator.Validate(request);


        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.TRANSACTION_TYPE_NOT_SUPPORTED); 
    }
}