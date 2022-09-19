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
            "56E0854E61C3468580E491C38405580B",
            "STAG_GPI",
            "4ucmqplxb029",
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
