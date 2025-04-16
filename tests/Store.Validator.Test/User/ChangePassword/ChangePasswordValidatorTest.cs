using CommonTestUtilities.Requests;
using Store.Application.UseCases.User.ChangePassword;
using Store.Exceptions;

namespace Store.Validator.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.INVALID_PASSWORD); 
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.PASSWORD_EMPTY);
         
    }
}
