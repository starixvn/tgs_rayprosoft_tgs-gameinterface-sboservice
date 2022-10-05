using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class UpdateAgentPresetBetSettingsUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var username = "STAG_GPI_CNY";
        var min = 1;
        var max = 5000;
        var maxPerMatch = 20000;
        var casinoTableLimit = 1;

        var url = new Uri(apiUrl, "/web-root/restricted/agent/update-agent-preset-bet-settings.aspx");

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

        var actual = await client
                .UpdateAgentPresetBetSettingsAsync(
                    apiUrl,
                    companyKey,
                    username,
                    min,
                    max,
                    maxPerMatch,
                    casinoTableLimit)
               .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsAgentExist);
    }

    [TestMethod]
    public async Task Agent_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var username = "STAG_GPI_CNY";
        var min = 1;
        var max = 5000;
        var maxPerMatch = 20000;
        var casinoTableLimit = 1;

        var url = new Uri(apiUrl, "/web-root/restricted/agent/update-agent-preset-bet-settings.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":4101,\"msg\":\"Agent Not Exists\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                .UpdateAgentPresetBetSettingsAsync(
                    apiUrl,
                    companyKey,
                    username,
                    min,
                    max,
                    maxPerMatch,
                    casinoTableLimit)
               .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.IsAgentExist);
    }
}
