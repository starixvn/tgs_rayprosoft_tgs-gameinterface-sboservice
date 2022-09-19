using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class GameUrlByVirtualSportsGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlByVirtualSportsQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            "STAG_GPI",
            "4ucmqplxb029",
            "en");

        var response = new LoginResponse()
        {
            IsPlayerExist = true,
            GameUrl = "sports-test.mydomain.com/welcome2.aspx?token=15433.sWRjsIETIDMjlZVTnzdQMNpV"
        };

        _ = fakeGameApiClient
             .LoginAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.CompanyKey),
                Arg.Is($"{query.Agent.Split('_')[0]}_{query.Username}"),
                Arg.Is("VirtualSports"))
             .Returns(response);

        var service = new GameUrlByVirtualSportsGetService(fakeGameApiClient);

        var actual = await service
                .GetAsync(
                    query)
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.AreEqual($"https://{response.GameUrl}&lang={query.Lang}", actual.GameUrl);
    }
}
