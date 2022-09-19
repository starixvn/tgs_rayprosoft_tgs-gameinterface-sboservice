using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class PlayerCreateGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerCreateCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            "4ucmqplxb029",
            "STAG_GPI");

        _ = fakeGameApiClient
             .RegisterPlayerAsync(
                Arg.Is(command.ApiUrl),
                Arg.Is(command.CompanyKey),
                Arg.Is($"{command.Agent.Split('_')[0]}_{command.Username}"),
                Arg.Is(command.Agent));

        var service = new PlayerCreateCommandService(fakeGameApiClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);
    }
}
