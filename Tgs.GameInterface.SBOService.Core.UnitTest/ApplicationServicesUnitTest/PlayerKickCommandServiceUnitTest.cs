using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class PlayerKickCommandServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerKickCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            "STAG_GPI",
            "4ucmqplxb029");

        _ = fakeGameApiClient
                .LogoutAsync(
                    Arg.Is(command.ApiUrl),
                    Arg.Is(command.CompanyKey),
        Arg.Is($"{command.Agent.Split('_')[0]}_{command.Username}"));

        var service = new PlayerKickCommandService(fakeGameApiClient);

        await service
                .ExecuteAsync(
                    command)
                .ConfigureAwait(false);
    }
}
