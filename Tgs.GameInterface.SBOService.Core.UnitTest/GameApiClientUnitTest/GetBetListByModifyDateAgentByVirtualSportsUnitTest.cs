using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class GetBetListByModifyDateAgentByVirtualSportsUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var agent = "TGS";
        var startTime = DateTime.Parse("2022-09-22 16:30:00 +08:00");
        var endTime = DateTime.Parse("2022-09-22 17:00:00 +08:00");

        var url = new Uri(apiUrl, "/web-root/restricted/report/get-bet-list-by-modify-date.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"result\":[{\"gameId\":2683156,\"odds\":-0.64,\"oddsStyle\":\"Malay\",\"actualStake\":1.92,\"turnover\":3.0,\"productType\":\"VirtualFootballDesktop\",\"subBet\":[{\"htScore\":\"1:0\",\"ftScore\":\"1:0\",\"betOption\":\"VL Bern\",\"marketType\":\"AsianHandicap\",\"hdp\":\"0\",\"odds\":-0.64,\"match\":\"VL Bern -vs- VL Zagreb\",\"status\":\"Won\",\"winLostDate\":\"2022-09-22T00:00:00\"}],\"orderTime\":\"2022-09-22T04:19:40.217\",\"modifyDate\":\"2022-09-22T04:54:37.643\",\"settleTime\":\"2022-09-22T04:54:28.963\",\"winLostDate\":\"2022-09-22T00:00:00\",\"refNo\":\"B2683156\",\"username\":\"STAG_4ucmqplxkuyd\",\"currency\":\"CNY\",\"stake\":3.0,\"winLost\":3.0,\"status\":\"won\",\"topDownline\":\"TGS\"}],\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                  .GetBetListByModifyDateAgentByVirtualSportsAsync(
                      apiUrl,
                      companyKey,
                      agent,
                      startTime,
                      endTime)
                 .ToArrayAsync()
                 .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }

    [TestMethod]
    public async Task Normal_Process_Unsettlement()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
        var agent = "TGS";
        var startTime = DateTime.Parse("2022-09-22 16:58:00 +08:00");
        var endTime = DateTime.Parse("2022-09-22 17:00:00 +08:00");

        var url = new Uri(apiUrl, "/web-root/restricted/report/get-bet-list-by-modify-date.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"result\":[{\"gameId\":2683157,\"odds\":0.16,\"oddsStyle\":\"Malay\",\"actualStake\":3.0,\"turnover\":3.0,\"productType\":\"VirtualFootballDesktop\",\"subBet\":[{\"htScore\":\"0:0\",\"ftScore\":\"0:0\",\"betOption\":\"Over\",\"marketType\":\"FirstHalfOverUnder\",\"hdp\":\"0.5\",\"odds\":0.16,\"match\":\"VL Amsterdam -vs- VL Prague\",\"status\":\"Running\",\"winLostDate\":\"2022-09-22T00:00:00\"}],\"orderTime\":\"2022-09-22T04:59:15.467\",\"modifyDate\":\"2022-09-22T04:59:17.683\",\"settleTime\":\"2022-09-22T04:59:15.543\",\"winLostDate\":\"0001-01-01T00:00:00\",\"refNo\":\"B2683157\",\"username\":\"STAG_4ucmqplxkuyd\",\"currency\":\"CNY\",\"stake\":3.0,\"winLost\":0.0,\"status\":\"running\",\"topDownline\":\"TGS\"}],\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                  .GetBetListByModifyDateAgentByVirtualSportsAsync(
                      apiUrl,
                      companyKey,
                      agent,
                      startTime,
                      endTime)
                 .ToArrayAsync()
                 .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
        Assert.AreEqual("running", actual[0].Status);
    }
}
