using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class GameHistoryBySportsBookQueryServiceUnitTest
{
    [TestMethod]
    public async Task Status_Is_Not_In_Filter()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-09-22 16:30:00 +08:00"),
            DateTime.Parse("2022-09-22 17:00:00 +08:00"));

        var response = new BetBySportsBookResponse(
            ExternalRoundId: "4471911",
            PlayerCode: "4ucmqplxkuyd",
            SportsType: "Football",
            Status: "waiting",
            OrderTime: DateTime.Parse("2022-09-22T04:19:40.217-04:00"),
            ModifyDate: DateTime.Parse("2022-09-22T04:33:22.737-04:00"),
            SettleTime: DateTime.Parse("2022-09-22T04:59:29.09-04:00"),
            WinLostDate: DateTime.Parse("2022-09-22T00:00:00-04:00"),
            Bet: 3m,
            ValidBet: 1.92m,
            Turnover: 3,
            PlayerWinLoss: 3,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "4471911" })
        );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetBySportsBookResponse[] { response
             }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.Any());
    }

    [TestMethod]
    public async Task Status_Is_Won()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-09-22 16:30:00 +08:00"),
            DateTime.Parse("2022-09-22 17:00:00 +08:00"));

        var response = new BetBySportsBookResponse(
            ExternalRoundId: "72344677",
            PlayerCode: "4ucmqplxkuyd",
            SportsType: "Football",
            Status: "won",
            OrderTime: DateTime.Parse("2022-10-30T00:16:37.6-04:00"),
            ModifyDate: DateTime.Parse("2022-10-30T00:52:11.917-04:00"),
            SettleTime: DateTime.Parse("2022-10-30T00:52:11.917-04:00"),
            WinLostDate: DateTime.Parse("2022-10-30T00:00:00.000-04:00"),
            Bet: 1000.0m,
            ValidBet: 1000.0m,
            Turnover: 1000.0m,
            PlayerWinLoss: 860.0m,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "72344677" })
        );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetBySportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.PlayerCode, actual[0].PlayerCode);
        Assert.AreEqual(response.ExternalRoundId, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Bet, actual[0].Bet);
        Assert.AreEqual(response.ValidBet, actual[0].ValidBet);
        Assert.AreEqual(response.PlayerWinLoss, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.PlayerWinLoss + response.ValidBet, actual[0].Settle);
    }

    [TestMethod]
    public async Task Status_Is_Lose()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-30T00:39:43.417-04:00"),
            DateTime.Parse("2022-10-30T02:06:53.25-04:00"));

        var response = new BetBySportsBookResponse(
            ExternalRoundId: "72347399",
            PlayerCode: "4uhiw6g8yyn1",
            SportsType: "Football",
            Status: "lose",
            OrderTime: DateTime.Parse("2022-10-30T00:39:43.417-04:00"),
            ModifyDate: DateTime.Parse("2022-10-30T02:06:53.25-04:00"),
            SettleTime: DateTime.Parse("2022-10-30T02:06:53.25-04:00"),
            WinLostDate: DateTime.Parse("2022-10-30T00:00:00.000-04:00"),
            Bet: 500.0m,
            ValidBet: 500.0m,
            Turnover: 500.0m,
            PlayerWinLoss: -500.0m,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "72347399" })
        );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetBySportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.PlayerCode, actual[0].PlayerCode);
        Assert.AreEqual(response.ExternalRoundId, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Bet, actual[0].Bet);
        Assert.AreEqual(response.ValidBet, actual[0].ValidBet);
        Assert.AreEqual(response.PlayerWinLoss, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(0, actual[0].Settle);
    }

    [TestMethod]
    public async Task Status_Is_Half_Lose()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-30T00:24:01.393-04:00"),
            DateTime.Parse("2022-10-30T00:49:35.797-04:00"));

        var response = new BetBySportsBookResponse(
            ExternalRoundId: "72345565",
            PlayerCode: "4uhiw6g8yyn1",
            SportsType: "Football",
            Status: "lose",
            OrderTime: DateTime.Parse("2022-10-30T00:24:01.393-04:00"),
            ModifyDate: DateTime.Parse("2022-10-30T00:49:35.797-04:00"),
            SettleTime: DateTime.Parse("2022-10-30T00:49:35.797-04:00"),
            WinLostDate: DateTime.Parse("2022-10-30T00:00:00.000-04:00"),
            Bet: 683.0m,
            ValidBet: 683.0m,
            Turnover: 683.0m,
            PlayerWinLoss: -341.5m,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "72345565" })
        );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetBySportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.PlayerCode, actual[0].PlayerCode);
        Assert.AreEqual(response.ExternalRoundId, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Bet, actual[0].Bet);
        Assert.AreEqual(response.ValidBet, actual[0].ValidBet);
        Assert.AreEqual(response.PlayerWinLoss, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.PlayerWinLoss + response.ValidBet, actual[0].Settle);
    }

    [TestMethod]
    public async Task Status_Is_Draw()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-27T05:21:44.907-04:00"),
            DateTime.Parse("2022-10-27T05:51:29.67-04:00"));

        var response = new BetBySportsBookResponse(
            ExternalRoundId: "71806058",
            PlayerCode: "4uhiw6g8yyn1",
            SportsType: "Football",
            Status: "draw",
            OrderTime: DateTime.Parse("2022-10-27T05:21:44.907-04:00"),
            ModifyDate: DateTime.Parse("2022-10-27T05:51:29.67-04:00"),
            SettleTime: DateTime.Parse("2022-10-27T05:51:29.67-04:00"),
            WinLostDate: DateTime.Parse("2022-10-27T00:00:00.000-04:00"),
            Bet: 675.0m,
            ValidBet: 675.0m,
            Turnover: 675.0m,
            PlayerWinLoss: 0.0m,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "71806058" })
        );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetBySportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.PlayerCode, actual[0].PlayerCode);
        Assert.AreEqual(response.ExternalRoundId, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Bet, actual[0].Bet);
        Assert.AreEqual(response.ValidBet, actual[0].ValidBet);
        Assert.AreEqual(response.PlayerWinLoss, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.PlayerWinLoss + response.ValidBet, actual[0].Settle);
    }
}
