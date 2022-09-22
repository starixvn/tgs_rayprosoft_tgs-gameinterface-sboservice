using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class PlayerBalanceGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new PlayerBalanceQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            "4ucmqplxkuyd");

        var response = new GetPlayerBalanceResponse()
        {
            IsPlayerExist = true,
            Balance = 100,
            Outstanding = 1
        };

        _ = fakeGameApiClient
             .GetPlayerBalanceAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is($"{query.Agent.Split('_')[0]}_{query.Username}"))
             .Returns(response);

        var service = new PlayerBalanceGetService(fakeGameApiClient);

        var actual = await service
                .GetAsync(
                    query)
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.AreEqual(99m, actual.Balance);
    }
}
