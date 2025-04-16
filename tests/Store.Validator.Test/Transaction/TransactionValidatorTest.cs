using CommonTestUtilities.Requests;
using Store.Communication.Enums;
using Store.Application.UseCases.Transaction;
using Store.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Store.Validator.Test.Transaction;

public class TransactionValidatorTest
{
    [Fact]
    public void Validator_Transaction_Success()
    {
        var validator = new TransactionValidator();

        var request = RequestTransactionJsonBuilder.Build();

        var result = validator.Validate(request);
         
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_Transaction_Error_TransactionType_TRANSACTION_TYPE_NOT_SUPPORTED()
    {
        var validator = new TransactionValidator();
        var request = RequestTransactionJsonBuilder.Build();

        request.TransactionType = (TransactionType)30;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.TRANSACTION_TYPE_NOT_SUPPORTED);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("          ")]
    [InlineData("")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because it is a unit test")]
    public void Validator_Transaction_Error_Description_DESCRIPTION_NOT_EMPTY(string description)
    {
        var validator = new TransactionValidator();
        var request = RequestTransactionJsonBuilder.Build();

        request.Description = description;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.TRANSACTION_DESCRIPTION_EMPTY);
    }

    [Fact]
    public void Validator_Transaction_Error_Amount_AMOUNT_GREATER_THAN_ZERO()
    {
        var validator = new TransactionValidator();
        var request = RequestTransactionJsonBuilder.Build();

        request.Amount = 0;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.AMOUNT_GREATER_THAN_ZERO);
    }

    [Fact]
    public void Validator_Transaction_Error_Amount_AMOUNT_MAXIMUM_TWO_UP_TO_TEN_DIGITS()
    {
        var validator = new TransactionValidator();
        var request = RequestTransactionJsonBuilder.Build();

        request.Amount = 045.0000m;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.AMOUNT_MAXIMUM_TWO_UP_TO_TEN_DIGITS);
    }

     
}
