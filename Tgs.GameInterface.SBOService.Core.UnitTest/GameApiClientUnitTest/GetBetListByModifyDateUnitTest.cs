using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class GetBetListByModifyDateUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
        var companyKey = "56E0854E61C3468580E491C38405580B";
        var portfolio = "SportsBook";

        var startDate = DateTime.Parse("2022-09-14 13:05:00 +08:00");
        var endDate = DateTime.Parse("2022-09-14 13:10:00 +08:00");

        var url = new Uri(apiUrl, "/web-root/restricted/report/v2/get-bet-list-by-modify-date.aspx");

        var handler = new MockHttpMessageHandler();
        _ = handler
                .Expect(HttpMethod.Post, url.ToString())
                .Respond(
                    HttpStatusCode.OK,
                    new StringContent("{\"result\":[{\"subBet\":[{\"betOption\":\"Hyderabad FC (R)\",\"marketType\":\"Handicap\",\"hdp\":-0.25,\"odds\":-0.87,\"league\":\"India 2nd Division League\",\"match\":\"Hyderabad FC (R) vs Bengaluru FC (R)\",\"status\":\"lose\",\"winlostDate\":\"2020-03-12T00:00:00\",\"liveScore\":\"0:0\",\"htScore\":\"0:0\",\"ftScore\":\"0:1\",\"customeizedBetType\":\"\",\"kickOffTime\":\"0001-01-01T00:00:00\",\"isHalfWonLose\":false}],\"refNo\":\"4000289\",\"username\":\"p0312\",\"sportsType\":\"Football\",\"orderTime\":\"2020-03-12T06:47:32.297\",\"winLostDate\":\"2020-03-12T00:00:00\",\"modifyDate\":\"2020-03-12T07:57:22.52\",\"odds\":-0.87,\"oddsStyle\":\"M\",\"stake\":8,\"actualStake\":6.96,\"currency\":\"TMP\",\"status\":\"lose\",\"winLost\":-6.96,\"turnover\":8,\"turnoverByStake\":8,\"turnoverByActualStake\":6.96,\"netTurnoverByStake\":8,\"netTurnoverByActualStake\":6.96,\"isHalfWonLose\":false,\"isLive\":true,\"maxWinWithoutActualStake\":8,\"topDownline\":\"YourApiDownline\",\"ip\":\"string\",\"voidReason\":\"RiskControl-CR\"},{\"subBet\":[{\"betOption\":\"Gremio Porto Alegre\",\"marketType\":\"Handicap\",\"hdp\":0,\"odds\":0.6,\"league\":\"COPA LIBERTADORES\",\"match\":\"Gremio Porto Alegre vs Internacional RS\",\"status\":\"draw\",\"winlostDate\":\"2020-03-12T00:00:00\",\"liveScore\":\"0:0\",\"htScore\":\"0:0\",\"ftScore\":\"0:0\",\"customeizedBetType\":\"\",\"kickOffTime\":\"0001-01-01T00:00:00\",\"isHalfWonLose\":false}],\"refNo\":\"4000338\",\"username\":\"p0312\",\"sportsType\":\"Football\",\"orderTime\":\"2020-03-12T21:44:42.677\",\"winLostDate\":\"2020-03-12T00:00:00\",\"modifyDate\":\"2020-03-12T22:08:58.507\",\"odds\":0.6,\"oddsStyle\":\"M\",\"stake\":10,\"actualStake\":10,\"currency\":\"TMP\",\"status\":\"draw\",\"winLost\":0,\"turnover\":10,\"turnoverByStake\":10,\"turnoverByActualStake\":10,\"netTurnoverByStake\":0,\"netTurnoverByActualStake\":0,\"isHalfWonLose\":false,\"isLive\":true,\"maxWinWithoutActualStake\":6,\"topDownline\":\"YourApiDownline\",\"ip\":\"string\"}],\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
                  .GetBetListByModifyDateAsync(
                      apiUrl,
                      companyKey,
                      portfolio,
                      startDate,
                      endDate)
                 .ToArrayAsync()
                 .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }
}
