using NSubstitute;
using Snowflake;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class FundOutGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();
        var fakeSnowFlake = new SnowFlake(datacenterId: 1, machineId: 1);

        var query = new FundOutQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            "4ucmqplxkuyd",
            1);

        var response = new TransactionResponse()
        {
            Refno = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            TxnId = "123465",
            Balance = 1,
            Outstanding = 0
        };

        _ = fakeGameApiClient
             .DepositAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is($"{query.Agent.Split('_')[0]}_{query.Username}"),
                Arg.Any<string>(),
                Arg.Is(query.Amount),
                Arg.Any<CancellationToken>())
             .Returns(response);

        var service = new FundOutGetService(
            fakeGameApiClient,
            fakeSnowFlake);

        var actual = await service
                .GetAsync(
                    query)
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(1m, actual.Balance);
    }
}
