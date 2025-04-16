using CommonTestUtilities.Requests;
using Store.Communication.Requests;
using Store.Exceptions;
using Store.WebAPI.Test.InLineData;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Store.WebAPI.Test.Login.DoLogin
{
    public class DoLoginTest : StoreClassFixture
    {
        private readonly string method = "login";

        private readonly string _password;
        private readonly string _email;
        private readonly string _name;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {

            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _name = factory.GetName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost(method, request);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            Assert.True(responseData.RootElement.GetProperty("name").GetString() == _name);
            Assert.NotEmpty(responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInLineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();


            var response = await DoPost(method, request, culture);

            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            Assert.True(errors.Single().GetString()!.Equals(expectedMessage));
        }
    }
}
