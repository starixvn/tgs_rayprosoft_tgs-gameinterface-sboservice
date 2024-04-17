using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SBOService.Core.UnitTest;

public class GetBetListByModifyDateAgentBySportsBookUnitTest
{
	[Fact]
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

	[Fact]
	public async Task Normal_Process_Unsettlement()
	{
		var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
		var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
		var agent = "TGS";
		var startTime = DateTime.Parse("2022-09-22 17:02:00 +08:00");
		var endTime = DateTime.Parse("2022-09-22 17:04:00 +08:00");

		var url = new Uri(apiUrl, "/web-root/restricted/report/get-bet-list-by-modify-date.aspx");

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, url.ToString())
			.Respond(
			HttpStatusCode.OK,
			new StringContent("{\"result\":[{\"subBet\":[{\"betOption\":\"e-Argentina\",\"marketType\":\"Handicap\",\"sportType\":\"Football\",\"hdp\":0.0,\"odds\":0.93,\"league\":\"e-Football F22 International Friendly\",\"match\":\"e-Brazil vs e-Argentina\",\"status\":\"running\",\"winlostDate\":\"2022-09-22T00:00:00\",\"liveScore\":\"0:0\",\"htScore\":\"0:0\",\"ftScore\":\"0:0\",\"customeizedBetType\":\"\",\"kickOffTime\":\"2022-09-22T04:45:00\",\"isHalfWonLose\":false}],\"sportsType\":\"Football\",\"odds\":0.93,\"oddsStyle\":\"M\",\"isLive\":true,\"maxWinWithoutActualStake\":4.65,\"ip\":\"220.132.206.70\",\"isSystemTagRisky\":false,\"isCustomerTagRisky\":false,\"voidReason\":\"\",\"actualStake\":5.0,\"turnover\":5.0,\"isHalfWonLose\":false,\"turnoverByStake\":5.0,\"turnoverByActualStake\":5.0,\"netTurnoverByStake\":0.0,\"netTurnoverByActualStake\":0.0,\"orderTime\":\"2022-09-22T05:03:09.2\",\"modifyDate\":\"2022-09-22T05:03:12.757\",\"settleTime\":\"2022-09-22T05:03:11.387\",\"winLostDate\":\"0001-01-01T00:00:00\",\"refNo\":\"4471967\",\"username\":\"STAG_4ucmqplxkuyd\",\"currency\":\"CNY\",\"stake\":5.0,\"winLost\":0.0,\"status\":\"running\",\"topDownline\":\"TGS\"}],\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

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
		Assert.Equal("4471967", actual[0].ExternalWagerId);
	}
}
