using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using System.Net;

namespace Store.WebAPI.Test.Transaction.Filter;

public class FilterTransactionInvalidTokenTest : StoreClassFixture
{
    private const string METHOD = "transaction/filter";

    public FilterTransactionInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestFilterTransactionJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, "tokenInvalid");

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestFilterTransactionJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPost(method: METHOD, request: request, token);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }
}