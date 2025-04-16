using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using System.Net;

namespace Store.WebAPI.Test.Transaction.Register;

public class RegisterTransactionInvalidTokenTest : StoreClassFixture
{
    private const string METHOD = "transaction";

    public RegisterTransactionInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestRegisterTransactionFormDataBuilder.Build();

        var response = await DoPostFormData(method: METHOD, request: request, token: "tokenInvalid");

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestRegisterTransactionFormDataBuilder.Build();

        var response = await DoPostFormData(method: METHOD, request: request, token: string.Empty);
         
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestRegisterTransactionFormDataBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPostFormData(method: METHOD, request: request, token: token);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }
}
