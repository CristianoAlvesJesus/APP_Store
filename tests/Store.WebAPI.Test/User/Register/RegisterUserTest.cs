using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Store.WebAPI.Test.InLineData;
using Xunit;
using System.Net.Http.Json;
using Store.Exceptions;

namespace Store.WebAPI.Test.Use.Register;

public class RegisterUserTest : StoreClassFixture
{
    private readonly string method = "user";



    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(method, request);

        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        Assert.True(responseData.RootElement.GetProperty("name").GetString() == request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
         
          
        var response = await DoPost(method: method, request: request, culture: culture);

        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        Assert.Equal(errors.Single().GetString()!, expectedMessage); 
    }
}
