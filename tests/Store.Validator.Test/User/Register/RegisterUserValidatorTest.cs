using CommonTestUtilities.Requests; 
using Store.Application.UseCases.User.Register;
using Store.Exceptions;
using Xunit;

namespace Store.Validator.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Sucess()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        Assert.False(result.IsValid); 
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.NAME_EMPTY); 
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        Assert.False(result.IsValid); 
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.EMAIL_EMPTY); 
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count == 1);
        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.EMAIL_INVALID);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);
        Assert.False(result.IsValid);

        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.INVALID_PASSWORD);
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        Assert.Contains(result.Errors, item => item.ErrorMessage == ResourceMessageException.PASSWORD_EMPTY); 
    }
}
