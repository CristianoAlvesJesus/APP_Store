using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Store.Communication.Requests;
using Store.Exceptions;
using Store.WebAPI.Test.InLineData;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Store.WebAPI.Test.Transaction.Filter;

public class FilterTransactionTest : StoreClassFixture
{
    private const string METHOD = "transaction/filter";

    private readonly Guid _userIdentifier;

    private string _description;
    private Store.Domain.Enums.TransactionType _transactionType;
    private decimal _amount;

    public FilterTransactionTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();

        _description = factory.GetTransactionDescription();

        _transactionType = factory.GetTransactionTransactionType();
        _amount = factory.GetTransactionAmount();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestFilterTransactionJson
        {
            Description = _description,
            TransactionType = (Store.Communication.Enums.TransactionType)_transactionType,
            Amount = _amount
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
         
        Assert.True(responseData.RootElement.GetProperty("transactions").EnumerateArray().Count() > 0);
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestFilterTransactionJsonBuilder.Build();
        request.Description = "transkk";

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_TransactionType_Invalid(string culture)
    {
        var request = RequestFilterTransactionJsonBuilder.Build();
        request.TransactionType = ((Store.Communication.Enums.TransactionType)10);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token, culture);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("TRANSACTION_TYPE_NOT_SUPPORTED", new CultureInfo(culture));

        Assert.Equal(errors.Single().GetString()!, expectedMessage);
    }
}