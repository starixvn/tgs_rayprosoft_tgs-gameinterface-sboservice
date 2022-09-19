using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class LogoutUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";

        var url = new Uri(apiUrl, "/web-root/restricted/player/logout.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"username\":\"4ucmqplxb029\",\"currency\":\"CNY\",\"balance\":1,\"outstanding\":0,\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        await client
               .LogoutAsync(
                   apiUrl,
                   companyKey,
                   username)
               .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";

        var url = new Uri(apiUrl, "/web-root/restricted/player/logout.aspx");

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

        await client
               .LogoutAsync(
                   apiUrl,
                   companyKey,
                   username)
               .ConfigureAwait(false);
    }
}
