using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class DepositUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var username = "4ucmqplxb029";
        var txnId = Guid.NewGuid().ToString("N");
        var amount = 1m;

        var url = new Uri(apiUrl, "/web-root/restricted/player/deposit.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"txnId\":\"D20190208190823090\",\"refno\":\"" + txnId + "\",\"balance\":1,\"outstanding\":0,\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                .DepositAsync(
                    apiUrl,
                    companyKey,
                    username,
                    txnId,
                    amount)
               .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(1m, actual.Balance);
    }
}
