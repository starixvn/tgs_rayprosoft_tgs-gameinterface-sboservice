using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class GameUrlBySportsBookGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            "STAG_GPI",
            "4ucmqplxb029",
            "en",
            false);

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
                Arg.Is("SportsBook"))
             .Returns(response);

        var service = new GameUrlBySportsBookGetService(fakeGameApiClient);

        var actual = await service
                .GetAsync(
                    query)
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.AreEqual($"https://{response.GameUrl}&lang={query.Lang}&oddstyle={query.Oddstyle}&theme={query.Theme}&oddsmode={query.Oddstyle}&device=d", actual.GameUrl);
    }
}
