using System.Globalization;
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
				statusCode: HttpStatusCode.OK,
				content: new StringContent("{\"result\":[{\"mainBetRef\":null,\"gamePeriodId\":\"Funky_fkg_300344324231\",\"gameRoundId\":\"Funky_fkg_300344324231\",\"gameType\":\"FunkyGames\",\"turnoverStake\":1.0,\"orderDetail\":\"\",\"gameResult\":\"Funky_fkg_300344324231\",\"gameId\":285,\"gpId\":16,\"isGameProviderPromotion\":false,\"orderTime\":\"2024-05-20T06:58:28.707\",\"modifyDate\":\"2024-05-20T06:58:36.077\",\"settleTime\":\"2024-05-20T06:58:36.077\",\"winLostDate\":\"2024-05-20T00:00:00.000\",\"refNo\":\"FunkyGames_55440870_Funky_fkg_300344324231\",\"username\":\"PROD_4umv4giizorc\",\"currency\":\"IDR\",\"stake\":1.0,\"winLost\":0.66,\"status\":\"won\",\"topDownline\":null}],\"serverId\":\"YY-production\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetBetListByModifyDateAgentBySeamlessGameAsync(
			apiUrl: apiUrl,
			companyKey: companyKey,
			agent: agent,
			startTime: startTime,
			endTime: endTime)
			.ToArrayAsync();

		Assert.NotEmpty(actual);
		Assert.Equal("Funky_fkg_300344324231", actual[0].ExternalRoundId);
		Assert.Equal("FunkyGames_55440870_Funky_fkg_300344324231", actual[0].ExternalWagerId);
		Assert.Equal("FunkyGames", actual[0].GameType);
		Assert.Equal("4umv4giizorc", actual[0].PlayerCode);
		Assert.Equal("won", actual[0].Status);
		Assert.Equal(DateTimeOffset.Parse("2024-05-20T06:58:28.707 -04:00"), actual[0].OrderTime);
		Assert.Equal(DateTimeOffset.Parse("2024-05-20T06:58:36.077  -04:00"), actual[0].ModifyDate);
		Assert.Equal(DateTimeOffset.Parse("2024-05-20T06:58:36.077  -04:00"), actual[0].SettleTime);
		Assert.Equal(DateTimeOffset.Parse("2024-05-20T00:00:00.000  -04:00"), actual[0].WinLostDate);
		Assert.Equal(1.0m, actual[0].Bet);
		Assert.Equal(1.0m, actual[0].Turnover);
		Assert.Equal(0.66m, actual[0].WinLoss);
	}
}
