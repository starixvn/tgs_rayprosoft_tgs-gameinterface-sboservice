using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;
public class GameWagerBySeamlessGameQueryServiceUnitTest
{
	[Fact]
	public async Task Status_Is_Won()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameWagerBySeamlessGameQuery(
			new Uri("https://ex-api-yy.xxttgg.com/"),
			"A54D5BCEEE204E4EB799E6BB4E617937",
			"PROD_SBO",
			DateTime.Parse("2024-05-20T00:00:00Z"),
			DateTime.Parse("2024-05-21T00:00:00Z"));

		var response = new BetBySeamlessGameResponse(
			ExternalRoundId: "Funky_fkg_300344324231",
			ExternalWagerId: "FunkyGames_55440870_Funky_fkg_300344324231",
			GameType: "FunkyGames",
			PlayerCode: "4umv4giizorc",
			Status: "won",
			OrderTime: DateTime.Parse("2024-05-20T06:58:28.707-04:00"),
			ModifyDate: DateTime.Parse("2024-05-20T06:58:36.077-04:00"),
			SettleTime: DateTime.Parse("2024-05-20T06:58:36.077-04:00"),
			WinLostDate: DateTime.Parse("2024-05-20T00:00:00.000-04:00"),
			Bet: 1.0m,
			Turnover: 1.0m,
			WinLoss: 0.66m,
			AdditionalData: JsonSerializer.Serialize(new { refNo = "FunkyGames_55440870_Funky_fkg_300344324231" })
		);

		_ = fakeGameApiClient
			 .GetBetListByModifyDateAgentBySeamlessGameAsync(
				Arg.Is(query.ApiUrl),
				Arg.Is(query.CompanyKey),
				Arg.Is(query.Agent),
				Arg.Is(query.StartTime),
				Arg.Is(query.EndTime))
			 .Returns(new BetBySeamlessGameResponse[] { response }.ToAsyncEnumerable());

		var service = new GameWagerBySeamlessGameQueryService(fakeGameApiClient);

		var actual = await service.QueryAsync(
			query)
			.ToArrayAsync();

		Assert.Equal("FunkyGames_55440870_Funky_fkg_300344324231", actual[0].ExternalWagerId);
		Assert.Equal("FunkyGames", actual[0].ExternalGameId);
		Assert.Equal("4umv4giizorc", actual[0].PlayerCode);
		Assert.Equal("won", actual[0].Status);
		Assert.Equal(DateTime.Parse("2024-05-20T06:58:28.707-04:00"), actual[0].ExternalCreatedOn);
		Assert.Equal(DateTime.Parse("2024-05-20T06:58:36.077-04:00"), actual[0].ExternalClosedOn);
		Assert.Equal(1.0m, actual[0].Bet);
		Assert.Equal(1.0m, actual[0].ValidBet);
		Assert.Equal(1.66m, actual[0].Settle);
		Assert.Equal(1.0m, actual[0].Turnover);
		Assert.Equal(0.66m, actual[0].PlayerWinLoss);
	}

	[Fact]
	public async Task Status_Is_Lose()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameWagerBySeamlessGameQuery(
			new Uri("https://ex-api-yy.xxttgg.com/"),
			"A54D5BCEEE204E4EB799E6BB4E617937",
			"PROD_SBO",
			DateTime.Parse("2024-05-20T00:00:00Z"),
			DateTime.Parse("2024-05-21T00:00:00Z"));

		var response = new BetBySeamlessGameResponse(
			ExternalRoundId: "Funky_fkg_300344323845",
			ExternalWagerId: "FunkyGames_55440870_Funky_fkg_300344323845",
			GameType: "FunkyGames",
			PlayerCode: "4umv4giizorc",
			Status: "lose",
			OrderTime: DateTime.Parse("2024-05-20T06:57:23.210-04:00"),
			ModifyDate: DateTime.Parse("2024-05-20T06:57:26.900-04:00"),
			SettleTime: DateTime.Parse("2024-05-20T06:57:26.900-04:00"),
			WinLostDate: DateTime.Parse("2024-05-20T00:00:00.000-04:00"),
			Bet: 50.0m,
			Turnover: 50.0m,
			WinLoss: -50.0m,
			AdditionalData: JsonSerializer.Serialize(new { refNo = "FunkyGames_55440870_Funky_fkg_300344323845" })
		);

		_ = fakeGameApiClient
			 .GetBetListByModifyDateAgentBySeamlessGameAsync(
				Arg.Is(query.ApiUrl),
				Arg.Is(query.CompanyKey),
				Arg.Is(query.Agent),
				Arg.Is(query.StartTime),
				Arg.Is(query.EndTime))
			 .Returns(new BetBySeamlessGameResponse[] { response }.ToAsyncEnumerable());

		var service = new GameWagerBySeamlessGameQueryService(fakeGameApiClient);

		var actual = await service.QueryAsync(
			query)
			.ToArrayAsync();

		Assert.Equal("FunkyGames_55440870_Funky_fkg_300344323845", actual[0].ExternalWagerId);
		Assert.Equal("FunkyGames", actual[0].ExternalGameId);
		Assert.Equal("4umv4giizorc", actual[0].PlayerCode);
		Assert.Equal("lose", actual[0].Status);
		Assert.Equal(DateTime.Parse("2024-05-20T06:57:23.210-04:00"), actual[0].ExternalCreatedOn);
		Assert.Equal(DateTime.Parse("2024-05-20T06:57:26.900-04:00"), actual[0].ExternalClosedOn);
		Assert.Equal(50.0m, actual[0].Bet);
		Assert.Equal(50.0m, actual[0].ValidBet);
		Assert.Equal(0.00m, actual[0].Settle);
		Assert.Equal(50.0m, actual[0].Turnover);
		Assert.Equal(-50.0m, actual[0].PlayerWinLoss);
	}
}
