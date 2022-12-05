using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class GameHistoryByVirtualSportsQueryServiceUnitTest
{
    [TestMethod]
    public async Task Status_Is_Not_In_Filter()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-09-22 16:30:00 +08:00"),
            DateTime.Parse("2022-09-22 17:00:00 +08:00"));

        var response = new BetByVirtualSportsResponse(
            ExternalRoundId: "B2683157",
            PlayerCode: "4ucmqplxkuyd",
            ProductType: "VirtualFootballDesktop",
            Status: "waiting",
            OrderTime: DateTimeOffset.Parse("2022-09-22T04:59:15.467-04:00"),
            ModifyDate: DateTimeOffset.Parse("2022-09-22T04:59:17.683-04:00"),
            SettleTime: DateTime.Parse("2022-09-22T04:59:15.543-04:00"),
            WinLostDate: DateTime.Parse("0001-01-01T00:00:00-04:00"),
            Bet: 3m,
            ValidBet: 3m,
            Turnover: 3m,
            PlayerWinLoss: 0.0m,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "B2683157" }));

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetByVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.Any());
    }

    [TestMethod]
    public async Task Status_Is_Running()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-09-22 16:30:00 +08:00"),
            DateTime.Parse("2022-09-22 17:00:00 +08:00"));

        var response = new BetByVirtualSportsResponse(
            ExternalRoundId: "B2683157",
            PlayerCode: "4ucmqplxkuyd",
            ProductType: "VirtualFootballDesktop",
            Status: "running",
            OrderTime: DateTimeOffset.Parse("2022-09-22T04:59:15.467-04:00"),
            ModifyDate: DateTimeOffset.Parse("2022-09-22T04:59:17.683-04:00"),
            SettleTime: DateTime.Parse("2022-09-22T04:59:15.543-04:00"),
            WinLostDate: DateTime.Parse("0001-01-01T00:00:00-04:00"),
            Bet: 3m,
            ValidBet: 3m,
            Turnover: 3m,
            PlayerWinLoss: 0.0m,
            AdditionalData: JsonSerializer.Serialize(new { refNo = "B2683157" }));

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetByVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

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
    public async Task Status_Is_Won()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-21T03:46:44.187-04:00"),
            DateTime.Parse("2022-10-21T03:49:51.92-04:00"));

        var response = new BetByVirtualSportsResponse(
                ExternalRoundId: "B4030894",
                PlayerCode: "4uhiw6g8yyn1",
                ProductType: "VirtualFootballMobile",
                Status: "won",
                OrderTime: DateTimeOffset.Parse("2022-10-21T03:46:44.187-04:00"),
                ModifyDate: DateTimeOffset.Parse("2022-10-21T05:21:05.4-04:00"),
                SettleTime: DateTimeOffset.Parse("2022-10-21T05:20:56.44-04:00"),
                WinLostDate: DateTimeOffset.Parse("2022-10-21T00:00:00-04:00"),
                Bet: 424.0m,
                ValidBet: 424.0m,
                Turnover: 424.0m,
                PlayerWinLoss: 127.2m,
                AdditionalData: JsonSerializer.Serialize("{\"gameId\":4030894,\"odds\":0.600,\"oddsStyle\":\"HK\",\"actualStake\":424.000000,\"turnover\":424.000000,\"productType\":\"VirtualFootballMobile\",\"subBet\":[{\"htScore\":\"0:0\",\"ftScore\":\"0:1\",\"betOption\":\"VL Madrid\",\"marketType\":\"AsianHandicap\",\"hdp\":\"-0.75\",\"odds\":0.600,\"match\":\"VL Bern -vs- VL Madrid\",\"status\":\"Won\",\"winLostDate\":\"2022-10-21T00:00:00-04:00\"}],\"orderTime\":\"2022-10-21T03:46:44.187-04:00\",\"modifyDate\":\"2022-10-21T05:21:05.4-04:00\",\"settleTime\":\"2022-10-21T05:20:56.44-04:00\",\"winLostDate\":\"2022-10-21T00:00:00-04:00\",\"refNo\":\"B4030894\",\"username\":\"PROD_4uhiw6g8yyn1\",\"currency\":\"HKD\",\"stake\":424.000000,\"winLost\":127.200000,\"status\":\"won\",\"topDownline\":null}")
            );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetByVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

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

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-24T00:39:44.2-04:00"),
            DateTime.Parse("2022-10-24T00:42:41.097-04:00"));

        var response = new BetByVirtualSportsResponse(
                ExternalRoundId: "B4039892",
                PlayerCode: "4uhiw6g8yyn1",
                ProductType: "VirtualFootballMobile",
                Status: "lose",
                OrderTime: DateTimeOffset.Parse("2022-10-24T00:39:44.2-04:00"),
                ModifyDate: DateTimeOffset.Parse("2022-10-24T00:42:51.557-04:00"),
                SettleTime: DateTimeOffset.Parse("2022-10-24T00:42:41.097-04:00"),
                WinLostDate: DateTimeOffset.Parse("2022-10-24T00:00:00-04:00"),
                Bet: 1000.0m,
                ValidBet: 1000.0m,
                Turnover: 1000.0m,
                PlayerWinLoss: -1000.0m,
                AdditionalData: JsonSerializer.Serialize("{\"gameId\":4039892,\"odds\":0.75,\"oddsStyle\":\"HK\",\"actualStake\":1000.0,\"turnover\":1000.0,\"productType\":\"VirtualFootballMobile\",\"subBet\":[{\"htScore\":\"0:1\",\"ftScore\":\"0:2\",\"betOption\":\"VL Madrid\",\"marketType\":\"AsianHandicap\",\"hdp\":\"-0.5\",\"odds\":0.75,\"match\":\"VL Madrid -vs- VL Lisbon\",\"status\":\"Lose\",\"winLostDate\":\"2022-10-24T00:00:00-04:00\"}],\"orderTime\":\"2022-10-24T00:39:44.2-04:00\",\"modifyDate\":\"2022-10-24T00:42:51.557-04:00\",\"settleTime\":\"2022-10-24T00:42:41.097-04:00\",\"winLostDate\":\"2022-10-24T00:00:00-04:00\",\"refNo\":\"B4039892\",\"username\":\"PROD_4uhiw6g8yyn1\",\"currency\":\"HKD\",\"stake\":1000.0,\"winLost\":-1000.0,\"status\":\"lose\",\"topDownline\":null}")
            );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetByVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

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
    public async Task Status_Is_Half_Won()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-24T00:39:44.2-04:00"),
            DateTime.Parse("2022-10-24T00:42:41.097-04:00"));

        var response = new BetByVirtualSportsResponse(
               ExternalRoundId: "B4091193",
               PlayerCode: "4uhiw6g8yyn1",
               ProductType: "VirtualFootballMobile",
               Status: "won",
               OrderTime: DateTimeOffset.Parse("2022-11-10T22:03:20.97-04:00"),
               ModifyDate: DateTimeOffset.Parse("2022-11-10T22:06:56.45-04:00"),
               SettleTime: DateTimeOffset.Parse("2022-11-10T22:06:46.43-04:00"),
               WinLostDate: DateTimeOffset.Parse("2022-11-10T00:00:00-04:00"),
               Bet: 665.0m,
               ValidBet: 665.0m,
               Turnover: 665.0m,
               PlayerWinLoss: 332.5m,
               AdditionalData: JsonSerializer.Serialize("{\"gameId\":4091193,\"odds\":0.5,\"oddsStyle\":\"HK\",\"actualStake\":665.0,\"turnover\":665.0,\"productType\":\"VirtualFootballMobile\",\"subBet\":[{\"htScore\":\"0:0\",\"ftScore\":\"1:0\",\"betOption\":\"VL Zagreb\",\"marketType\":\"AsianHandicap\",\"hdp\":\"0\",\"odds\":0.5,\"match\":\"VL Zagreb -vs- VL Athens\",\"status\":\"Won\",\"winLostDate\":\"2022-11-11T00:00:00-04:00\"}],\"orderTime\":\"2022-11-10T22:03:20.97-04:00\",\"modifyDate\":\"2022-11-10T22:06:56.45-04:00\",\"settleTime\":\"2022-11-10T22:06:46.43-04:00\",\"winLostDate\":\"2022-11-10T00:00:00-04:00\",\"refNo\":\"B4091193\",\"username\":\"PROD_4uhiw6g8yyn1\",\"currency\":\"HKD\",\"stake\":665.0,\"winLost\":332.5,\"status\":\"won\",\"topDownline\":null}")
           );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetByVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

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

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-21T03:57:38.057-04:00"),
            DateTime.Parse("2022-10-21T04:00:58.953-04:00"));

        var response = new BetByVirtualSportsResponse(
               ExternalRoundId: "B4030919",
               PlayerCode: "4uhiw6g8yyn1",
               ProductType: "VirtualFootballMobile",
               Status: "draw",
               OrderTime: DateTimeOffset.Parse("2022-10-21T03:57:38.057-04:00"),
               ModifyDate: DateTimeOffset.Parse("2022-10-21T05:21:05.4-04:00"),
               SettleTime: DateTimeOffset.Parse("2022-10-21T05:20:56.44-04:00"),
               WinLostDate: DateTimeOffset.Parse("2022-10-21T00:00:00-04:00"),
               Bet: 500.0m,
               ValidBet: 500.0m,
               Turnover: 500.0m,
               PlayerWinLoss: 0.0m,
               AdditionalData: JsonSerializer.Serialize("{\"gameId\":4030919,\"odds\":0.550,\"oddsStyle\":\"HK\",\"actualStake\":500.000000,\"turnover\":500.000000,\"productType\":\"VirtualFootballMobile\",\"subBet\":[{\"htScore\":\"1:0\",\"ftScore\":\"1:1\",\"betOption\":\"VL Kiev\",\"marketType\":\"AsianHandicap\",\"hdp\":\"0\",\"odds\":0.550,\"match\":\"VL Kiev -vs- VL Athens\",\"status\":\"Draw\",\"winLostDate\":\"2022-10-21T00:00:00-04:00\"}],\"orderTime\":\"2022-10-21T03:57:38.057-04:00\",\"modifyDate\":\"2022-10-21T05:21:05.4-04:00\",\"settleTime\":\"2022-10-21T05:20:56.44-04:00\",\"winLostDate\":\"2022-10-21T00:00:00-04:00\",\"refNo\":\"B4030919\",\"username\":\"PROD_4uhiw6g8yyn1\",\"currency\":\"HKD\",\"stake\":500.000000,\"winLost\":0.000000,\"status\":\"draw\",\"topDownline\":null}")
           );

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetByVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

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
