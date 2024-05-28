using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest;

public class GetBetListByModifyDateAgentBySeamlessGameUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://ex-api-yy.xxttgg.com/");
		var companyKey = "A54D5BCEEE204E4EB799E6BB4E617937";
		var agent = "PROD_SBO";
		var startTime = DateTime.Parse("2024-05-20 00:00:00 +08:00");
		var endTime = DateTime.Parse("2024-05-21 00:00:00 +08:00");

		var url = new Uri(apiUrl, "/web-root/restricted/report/get-bet-list-by-modify-date.aspx");

		var handler = new MockHttpMessageHandler();
		_ = handler
				.Expect(HttpMethod.Post, url.ToString())
				.Respond(
					HttpStatusCode.OK,
					new StringContent("{\"result\":[{\"subBet\":[{\"betOption\":\"e-Argentina\",\"marketType\":\"Handicap\",\"sportType\":\"Football\",\"hdp\":-0.25,\"odds\":0.82,\"league\":\"e-Football F22 International Friendly\",\"match\":\"e-Poland vs e-Argentina\",\"status\":\"lose\",\"winlostDate\":\"2022-09-22T00:00:00\",\"liveScore\":\"1:3\",\"htScore\":\"1:3\",\"ftScore\":\"1:3\",\"customeizedBetType\":\"\",\"kickOffTime\":\"2022-09-22T04:00:00\",\"isHalfWonLose\":true}],\"sportsType\":\"Football\",\"odds\":0.82,\"oddsStyle\":\"M\",\"isLive\":true,\"maxWinWithoutActualStake\":4.1,\"ip\":\"220.132.206.70\",\"isSystemTagRisky\":false,\"isCustomerTagRisky\":false,\"voidReason\":\"\",\"actualStake\":5.0,\"turnover\":5.0,\"isHalfWonLose\":true,\"turnoverByStake\":5.0,\"turnoverByActualStake\":5.0,\"netTurnoverByStake\":2.5,\"netTurnoverByActualStake\":2.5,\"orderTime\":\"2022-09-22T04:18:31.42\",\"modifyDate\":\"2022-09-22T04:33:22.737\",\"settleTime\":\"2022-09-22T04:33:15.573\",\"winLostDate\":\"2022-09-22T00:00:00\",\"refNo\":\"4471911\",\"username\":\"STAG_4ucmqplxkuyd\",\"currency\":\"CNY\",\"stake\":5.0,\"winLost\":-2.5,\"status\":\"lose\",\"topDownline\":\"TGS\"}],\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetBetListByModifyDateAgentBySportsBookAsync(
			apiUrl: apiUrl,
			companyKey: companyKey,
			agent: agent,
			startTime: startTime,
			endTime: endTime)
			.ToArrayAsync();

		Assert.NotEmpty(actual);
		Assert.Equal("4ucmqplxkuyd", actual[0].PlayerCode);
		Assert.Equal("4471911", actual[0].ExternalWagerId);
	}
}
