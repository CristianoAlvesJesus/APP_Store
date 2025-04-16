﻿using CommonTestUtilities.Tokens;
using System.Net;

namespace Store.WebAPI.Test.Use.Profile;

public class GetUserProfileInvalidTokenTest : StoreClassFixture
{
    private readonly string METHOD = "user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(METHOD, token: "tokenInvalid"); 

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(METHOD, token: string.Empty); 

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(METHOD, token);
          
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
