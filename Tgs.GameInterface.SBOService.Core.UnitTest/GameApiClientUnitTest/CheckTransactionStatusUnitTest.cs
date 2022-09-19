using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SBOService.Core.Exceptions;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class CheckTransactionStatusUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var txnId = Guid.NewGuid().ToString("N");

        var url = new Uri(apiUrl, "/web-root/restricted/player/check-transaction-status.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"amount\":8947,\"txnId\":\"W0128012380\",\"refno\":\"" + txnId + "\",\"transactionType\":\"withdraw\",\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                .CheckTransactionStatusAsync(
                     apiUrl,
                     companyKey,
                     txnId)
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public async Task Trade_Order_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var txnId = Guid.NewGuid().ToString("N");

        var url = new Uri(apiUrl, "/web-root/restricted/player/check-transaction-status.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":4401,\"msg\":\"Invalid Transaction Id\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        _ = await Assert.ThrowsExceptionAsync<GameProvisioningErrorException>(async ()
            => await client
                .CheckTransactionStatusAsync(
                    apiUrl,
                    companyKey,
                    txnId)
                .ConfigureAwait(false))
            .ConfigureAwait(false);
    }
}
