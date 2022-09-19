using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class LoginUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";
        var portfolio = "SportsBook";

        var url = new Uri(apiUrl, "/web-root/restricted/player/login.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"url\":\"sports-test.mydomain.com/welcome2.aspx?token=15433.sWRjsIETIDMjlZVTnzdQMNpV\",\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                .LoginAsync(
                   apiUrl,
                   companyKey,
                   username,
                   portfolio)
               .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.IsFalse(string.IsNullOrWhiteSpace(actual.GameUrl));
    }

    [TestMethod]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";
        var portfolio = "SportsBook";

        var url = new Uri(apiUrl, "/web-root/restricted/player/login.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":3303,\"msg\":\"User Doesn't Exist\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                .LoginAsync(
                   apiUrl,
                   companyKey,
                   username,
                   portfolio)
               .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.IsPlayerExist);
    }
}
