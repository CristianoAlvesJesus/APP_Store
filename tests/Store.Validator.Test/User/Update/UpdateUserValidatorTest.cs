using CommonTestUtilities.Requests;
using Store.Application.UseCases.User.Update;
using Store.Exceptions;

namespace Store.Validator.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.NotNull(result);
    }


[Fact]
    public void Error_Email_Empty()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();

        request.Email = string.Empty;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "teste.com";

        var result = validator.Validate(request);  


        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.EMAIL_INVALID);
    }

    [Fact]
    public void Error_NameEmpty()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request); 

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.NAME_EMPTY);
    }
}