using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SBOService.Core.Exceptions;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class RegisterPlayerUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";
        var agent = "STAG_GPI";

        var url = new Uri(apiUrl, "/web-root/restricted/player/register-player.aspx");

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
            .RegisterPlayerAsync(
                apiUrl,
                companyKey,
                username,
                agent)
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Agent_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";
        var agent = "STAG_GPI";

        var url = new Uri(apiUrl, "/web-root/restricted/player/register-player.aspx");

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

        _ = await Assert.ThrowsExceptionAsync<GameProvisioningErrorException>(async () =>
        {
            await client
                .RegisterPlayerAsync(
                    apiUrl,
                    companyKey,
                    username,
                    agent)
                .ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Player_Already_Exists()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";
        var agent = "STAG_GPI";

        var url = new Uri(apiUrl, "/web-root/restricted/player/register-player.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":4103,\"msg\":\"User Exists\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        await client
            .RegisterPlayerAsync(
                apiUrl,
                companyKey,
                username,
                agent)
            .ConfigureAwait(false);
    }
}
