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
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "4ucmqplxkuyd",
            "STAG_GPI_CNY");

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
