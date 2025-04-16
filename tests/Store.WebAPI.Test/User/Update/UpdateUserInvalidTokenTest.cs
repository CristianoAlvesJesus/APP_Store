using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using System.Net;

namespace Store.WebAPI.Test.User.Update;

public class UpdateUserInvalidTokenTest : StoreClassFixture
{
    private const string METHOD = "user";

    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication)
    {
    }

    [Fact]
    public async Task Token_Invalid_Error_Unauthorized()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(METHOD, request, token: "tokenInvalid"); 

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Token_Without_Error_Unauthorized()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(METHOD, request, token: string.Empty); 

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(METHOD, request, token);  
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); 
    }
}