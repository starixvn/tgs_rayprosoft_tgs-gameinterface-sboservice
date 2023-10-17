using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest;

public class LoginUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var username = "4ucmqplxkuyd";
        var portfolio = "SportsBook";

        var url = new Uri(apiUrl, "/web-root/restricted/player/login.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"url\":\"//virtualsports-winfast888.1StepRich.com/Home/Welcome?gmOnlineId=3458478272&token=8722482.GWxHdvRGmvuwyLRiuBoLDOR\",\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.LoginAsync(
            apiUrl: apiUrl,
            companyKey: companyKey,
            username: username,
            portfolio: portfolio);

        Assert.True(actual.IsPlayerExist);
        Assert.False(string.IsNullOrWhiteSpace(actual.GameUrl));
    }

    [Fact]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var username = "4ucmqplxkuyd";
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

        var actual = await client.LoginAsync(
            apiUrl: apiUrl,
            companyKey: companyKey,
            username: username,
            portfolio: portfolio);

        Assert.False(actual.IsPlayerExist);
    }
}
