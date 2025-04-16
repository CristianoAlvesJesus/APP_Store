using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Store.Exceptions;
using Store.WebAPI.Test.InLineData;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Store.WebAPI.Test.Transaction.Register;

public class RegisterTransactionTest : StoreClassFixture
{
    private const string METHOD = "transaction";

    private readonly Guid _userIdentifier;

    public RegisterTransactionTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterTransactionFormDataBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPostFormData(method: METHOD, request: request, token: token);
         
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        Assert.True(responseData.RootElement.GetProperty("description").GetString() == request.Description); 
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestRegisterTransactionFormDataBuilder.Build();
        request.Description = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPostFormData(method: METHOD, request: request, token: token, culture: culture);
         
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
         

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("TRANSACTION_DESCRIPTION_EMPTY", new CultureInfo(culture));

        Assert.Equal(errors.Single().GetString()!, expectedMessage);

        
    }
}
