using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Store.Communication.Requests;
using Store.Exceptions;
using Store.WebAPI.Test.InLineData;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Store.WebAPI.Test.User.ChangePassword;

public class ChangePasswordTest : StoreClassFixture
{
    private const string METHOD = "user/change-password";

    private readonly string _password;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.GetPassword();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = _password;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(METHOD, request, token);
         
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password,
        };

        response = await DoPost(method: "login", request: loginRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); 

        loginRequest.Password = request.NewPassword;

        response = await DoPost(method: "login", request: loginRequest); 
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_NewPassword_Empty(string culture)
    {
        var request = new RequestChangePasswordJson
        {
            CurrentPassword = _password,
            NewPassword = string.Empty
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(METHOD, request, token, culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture));
         
        Assert.Equal(errors.Single().GetString()!, expectedMessage);
    }
}