using Azure.Core;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using Store.Exceptions;
using Store.WebAPI.Test.InLineData;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Store.WebAPI.Test.Transaction.Gets;

public class GetTransactionByIdTest : StoreClassFixture
{
    private const string METHOD = "transaction";
    private readonly Guid _userIdentifier;
    private readonly string _transactionId;
    private readonly string _transactionDescription;

    public GetTransactionByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _transactionId = factory.GetTreansactionId();
        _transactionDescription = factory.GetTransactionDescription();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet($"{METHOD}/{_transactionId}", token);
         
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK); 

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);


        Assert.True(responseData.RootElement.GetProperty("id").GetString() == _transactionId);
        Assert.True(responseData.RootElement.GetProperty("description").GetString() == _transactionDescription); 
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Transaction_Not_Found(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var id = IdEncripterBuilder.Build().Encode(1000);

        var response = await DoGet($"{METHOD}/{id}", token, culture); 

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("TRANSACTION_NOT_FOUND", new CultureInfo(culture)); 

        Assert.Equal(errors.Single().GetString()!, expectedMessage);
    }
}
