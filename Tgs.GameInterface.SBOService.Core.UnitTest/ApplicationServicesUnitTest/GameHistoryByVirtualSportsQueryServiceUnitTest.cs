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

        var response = new BetVirtualSportsResponse()
        {
            Username = "STAG_4ucmqplxkuyd",
            RefNo = "B2683156",
            Status = "waiting",
            OrderTime = DateTime.Parse("2022-09-22T04:19:40.217-04:00"),
            SettleTime = DateTime.Parse("2022-09-22T04:59:29.09-04:00"),
            Stake = 3,
            ActualStake = 1.92m,
            WinLost = 3,
            Turnover = 3
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetVirtualSportsResponse[] { response }.ToAsyncEnumerable());

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
    public async Task Status_Is_Won()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-21T03:46:44.187-04:00"),
            DateTime.Parse("2022-10-21T03:49:51.92-04:00"));

        var response = new BetVirtualSportsResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "B4030894",
            Status = "won",
            OrderTime = DateTime.Parse("2022-10-21T03:46:44.187-04:00"),
            SettleTime = DateTime.Parse("2022-10-21T03:49:51.92-04:00"),
            Stake = 424.0m,
            ActualStake = 424.0m,
            WinLost = 127.2m,
            Turnover = 424.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.Username.Split("_")[1], actual[0].PlayerCode);
        Assert.AreEqual(response.RefNo, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Stake, actual[0].Bet);
        Assert.AreEqual(response.ActualStake, actual[0].ValidBet);
        Assert.AreEqual(response.WinLost, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.WinLost + response.ActualStake, actual[0].Settle);
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

        var response = new BetVirtualSportsResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "B4039892",
            Status = "lose",
            OrderTime = DateTime.Parse("2022-10-24T00:39:44.2-04:00"),
            SettleTime = DateTime.Parse("2022-10-24T00:42:41.097-04:00"),
            Stake = 1000.0m,
            ActualStake = 1000.0m,
            WinLost = -1000.0m,
            Turnover = 1000.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.Username.Split("_")[1], actual[0].PlayerCode);
        Assert.AreEqual(response.RefNo, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Stake, actual[0].Bet);
        Assert.AreEqual(response.ActualStake, actual[0].ValidBet);
        Assert.AreEqual(response.WinLost, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.WinLost + response.ActualStake, actual[0].Settle);
    }

    [TestMethod]
    public async Task Status_Is_Half_Lose()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameHistoryByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-24T00:39:44.2-04:00"),
            DateTime.Parse("2022-10-24T00:42:41.097-04:00"));

        var response = new BetVirtualSportsResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "B4039892",
            Status = "lose",
            OrderTime = DateTime.Parse("2022-10-24T00:39:44.2-04:00"),
            SettleTime = DateTime.Parse("2022-10-24T00:42:41.097-04:00"),
            Stake = 683.0m,
            ActualStake = 683.0m,
            WinLost = -341.5m,
            Turnover = 683.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.Username.Split("_")[1], actual[0].PlayerCode);
        Assert.AreEqual(response.RefNo, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Stake, actual[0].Bet);
        Assert.AreEqual(response.ActualStake, actual[0].ValidBet);
        Assert.AreEqual(response.WinLost, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.WinLost + response.ActualStake, actual[0].Settle);
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

        var response = new BetVirtualSportsResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "B4030919",
            Status = "draw",
            OrderTime = DateTime.Parse("2022-10-21T03:57:38.057-04:00"),
            SettleTime = DateTime.Parse("2022-10-21T04:00:58.953-04:00"),
            Stake = 500.0m,
            ActualStake = 500.0m,
            WinLost = 0.0m,
            Turnover = 500.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentByVirtualSportsAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetVirtualSportsResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryByVirtualSportsQueryService(fakeGameApiClient);

        var actual = await service
                .QueryAsync(
                    query)
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(response.Username.Split("_")[1], actual[0].PlayerCode);
        Assert.AreEqual(response.RefNo, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Stake, actual[0].Bet);
        Assert.AreEqual(response.ActualStake, actual[0].ValidBet);
        Assert.AreEqual(response.WinLost, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.WinLost + response.ActualStake, actual[0].Settle);
    }
}
