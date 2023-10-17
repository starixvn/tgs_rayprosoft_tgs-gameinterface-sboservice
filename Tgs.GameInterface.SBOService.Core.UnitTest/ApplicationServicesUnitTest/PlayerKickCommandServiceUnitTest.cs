using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class PlayerKickCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();
        var fakeUserNameUtensils = Substitute.For<IUserNameUtensils>();

        var command = new PlayerKickCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            "4ucmqplxkuyd");

        var username = "STAG_4ucmqplxkuyd";
        _ = fakeUserNameUtensils.GetUserName(
            agent: Arg.Is(command.Agent),
            playerCode: Arg.Is(command.PlayerCode))
            .Returns(username);

        var service = new PlayerKickCommandService(
            fakeGameApiClient,
            fakeUserNameUtensils);

        await service.ExecuteAsync(command);

        _ = fakeGameApiClient
            .Received(1)
            .LogoutAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            companyKey: Arg.Is(command.CompanyKey),
            username: Arg.Is(username));
    }
}
