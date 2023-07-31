using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class WithdrawUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var username = "4ucmqplxkuyd";
        var txnId = Guid.NewGuid().ToString("N");
        var amount = 1m;
        var isFullAmount = false;

        var url = new Uri(apiUrl, "/web-root/restricted/player/withdraw.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"txnId\":\"D20190208190823090\",\"refno\":\"" + txnId + "\",\"balance\":0,\"outstanding\":0,\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                .WithdrawAsync(
                    apiUrl,
                    companyKey,
                    username,
                    txnId,
                    amount,
                    isFullAmount)
               .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual("D20190208190823090", actual.TxnId);
    }
}
