using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class FundStatusGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            Guid.NewGuid().ToString("N"));

        _ = fakeGameApiClient
             .CheckTransactionStatusAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is(query.TxnId))
             .Returns(true);

        var service = new FundStatusGetService(fakeGameApiClient);

        var actual = await service
                .GetAsync(
                    query)
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual);
    }
}
