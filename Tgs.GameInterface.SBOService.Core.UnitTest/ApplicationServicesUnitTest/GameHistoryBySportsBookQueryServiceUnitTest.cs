using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class GameHistoryBySportsBookQueryServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
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
            Status = "won",
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
        Assert.AreEqual(response.Username.Split("_")[1], actual[0].PlayerCode);
        Assert.AreEqual(response.RefNo, actual[0].ExternalRoundId);
        Assert.AreEqual(response.OrderTime, actual[0].ExternalCreatedOn);
        Assert.AreEqual(response.SettleTime, actual[0].ExternalClosedOn);
        Assert.AreEqual(response.Stake, actual[0].Bet);
        Assert.AreEqual(response.ActualStake, actual[0].ValidBet);
        Assert.AreEqual(response.WinLost - response.ActualStake, actual[0].PlayerWinLoss);
        Assert.AreEqual(response.Turnover, actual[0].Turnover);
        Assert.AreEqual(response.WinLost, actual[0].Settle);
    }

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
}
