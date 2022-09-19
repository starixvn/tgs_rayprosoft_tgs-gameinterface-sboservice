using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class RegisterAgentUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "STAG_GPI";
        var currency = "CNY";
        var min = 1;
        var max = 5000;
        var maxPerMatch = 20000;
        var casinoTableLimit = 1;

        var url = new Uri(apiUrl, "/web-root/restricted/agent/register-agent.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        await client
                .RegisterAgentAsync(
                    apiUrl,
                    companyKey,
                    username,
                    currency,
                    min,
                    max,
                    maxPerMatch,
                    casinoTableLimit)
               .ConfigureAwait(false);
    }
}
