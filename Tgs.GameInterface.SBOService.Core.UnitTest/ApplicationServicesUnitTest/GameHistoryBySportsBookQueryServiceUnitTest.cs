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

        var response = new BetSportsBookResponse()
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
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetSportsBookResponse[] { response }.ToAsyncEnumerable());

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

        var response = new BetSportsBookResponse()
        {
            Username = "STAG_4ucmqplxkuyd",
            RefNo = "72344677",
            Status = "won",
            OrderTime = DateTime.Parse("2022-10-30T00:16:37.6-04:00"),
            SettleTime = DateTime.Parse("2022-10-30T00:52:11.917-04:00"),
            Stake = 1000.0m,
            ActualStake = 1000.0m,
            WinLost = 860.0m,
            Turnover = 1000.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetSportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

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

        var query = new GameHistoryBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            DateTime.Parse("2022-10-30T00:39:43.417-04:00"),
            DateTime.Parse("2022-10-30T02:06:53.25-04:00"));

        var response = new BetSportsBookResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "72347399",
            Status = "lose",
            OrderTime = DateTime.Parse("2022-10-30T00:39:43.417-04:00"),
            SettleTime = DateTime.Parse("2022-10-30T02:06:53.25-04:00"),
            Stake = 500.0m,
            ActualStake = 500.0m,
            WinLost = -500.0m,
            Turnover = 500.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetSportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

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

        var response = new BetSportsBookResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "72345565",
            Status = "lose",
            OrderTime = DateTime.Parse("2022-10-30T00:24:01.393-04:00"),
            SettleTime = DateTime.Parse("2022-10-30T00:49:35.797-04:00"),
            Stake = 683.0m,
            ActualStake = 683.0m,
            WinLost = -341.5m,
            Turnover = 683.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetSportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

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
        Assert.AreEqual(response.WinLost+ response.ActualStake, actual[0].Settle);
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

        var response = new BetSportsBookResponse()
        {
            Username = "PROD_4uhiw6g8yyn1",
            RefNo = "71806058",
            Status = "draw",
            OrderTime = DateTime.Parse("2022-10-27T05:21:44.907-04:00"),
            SettleTime = DateTime.Parse("2022-10-27T05:51:29.67-04:00"),
            Stake = 675.0m,
            ActualStake = 675.0m,
            WinLost = 0.0m,
            Turnover = 675.0m
        };

        _ = fakeGameApiClient
             .GetBetListByModifyDateAgentBySportsBookAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.Agent),
                Arg.Is(query.StartTime),
                Arg.Is(query.EndTime))
             .Returns(new BetSportsBookResponse[] { response }.ToAsyncEnumerable());

        var service = new GameHistoryBySportsBookQueryService(fakeGameApiClient);

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
