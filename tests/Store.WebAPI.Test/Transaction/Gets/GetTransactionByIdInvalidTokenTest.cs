using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using System.Net;

namespace Store.WebAPI.Test.Transaction.Gets;

public class GetTransactionByIdInvalidTokenTest : StoreClassFixture
{
    private const string METHOD = "transaction";

    public GetTransactionByIdInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet($"{METHOD}/{id}", token: "tokenInvalid");
         
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet($"{METHOD}/{id}", token: string.Empty);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet($"{METHOD}/{id}", token: token);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }
}