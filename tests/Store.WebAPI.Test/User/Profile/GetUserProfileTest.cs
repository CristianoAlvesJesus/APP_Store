using CommonTestUtilities.Tokens; 
using System.Net;
using System.Text.Json;
using Xunit;

namespace Store.WebAPI.Test.Use.Profile;

public class GetUserProfileTest: StoreClassFixture
{
    private readonly string METHOD = "user";

    private readonly string _name;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(METHOD, token: token); 

        Assert.Equal(HttpStatusCode.OK, response.StatusCode); ;

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        Assert.True(responseData.RootElement.GetProperty("name").GetString() == _name);
        Assert.True(responseData.RootElement.GetProperty("email").GetString() == _email);
    }
}