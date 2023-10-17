using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class GameUrlBySportsBookGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();
        var fakeUserNameUtensils = Substitute.For<IUserNameUtensils>();

        var query = new GameUrlBySportsBookQuery(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            "4ucmqplxkuyd",
            "en",
            false);

        var username = "STAG_4ucmqplxkuyd";
        _ = fakeUserNameUtensils.GetUserName(
            agent: Arg.Is(query.Agent),
            playerCode: Arg.Is(query.PlayerCode))
            .Returns(username);

        var response = new LoginResponse()
        {
            IsPlayerExist = true,
            GameUrl = "//sports-test.mydomain.com/welcome2.aspx?token=15433.sWRjsIETIDMjlZVTnzdQMNpV"
        };

        _ = fakeGameApiClient.LoginAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            companyKey: Arg.Is(query.CompanyKey),
            username: Arg.Is(username),
            portfolio: Arg.Is("SportsBook"))
             .Returns(response);

        var service = new GameUrlBySportsBookGetService(
            fakeGameApiClient,
            fakeUserNameUtensils);

        var actual = await service.GetAsync(query);

        Assert.True(actual.IsPlayerExist);
        Assert.Equal($"https:{response.GameUrl}&lang={query.Lang}&oddstyle={query.Oddstyle}&theme={query.Theme}&oddsmode={query.Oddstyle}&device=d", actual.GameUrl);
    }
}
