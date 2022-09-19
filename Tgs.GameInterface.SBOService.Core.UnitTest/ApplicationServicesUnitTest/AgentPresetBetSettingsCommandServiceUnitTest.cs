using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class AgentPresetBetSettingsCommandServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new AgentPresetBetSettingsCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            "STAG_GPI",
            "CNY",
            1,
            5000,
            20000,
            1);

        var response = new UpdateAgentPresetBetSettingsResponse()
        {
            IsAgentExist = true,
        };

        _ = fakeGameApiClient
             .UpdateAgentPresetBetSettingsAsync(
                Arg.Is(command.ApiUrl),
                Arg.Is(command.CompanyKey),
                Arg.Is(command.Username),
                Arg.Is(command.Min),
                Arg.Is(command.Max),
                Arg.Is(command.MaxPerMatch),
                Arg.Is(command.CasinoTableLimit))
             .Returns(response);

        var service = new AgentPresetBetSettingsCommandService(fakeGameApiClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        _ = fakeGameApiClient
             .Received(0)
             .RegisterAgentAsync(
                Arg.Is(command.ApiUrl),
                Arg.Is(command.CompanyKey),
                Arg.Is(command.Username),
                Arg.Is(command.Currency),
                Arg.Is(command.Min),
                Arg.Is(command.Max),
                Arg.Is(command.MaxPerMatch),
                Arg.Is(command.CasinoTableLimit));
    }

    [TestMethod]
    public async Task Register_Agent()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new AgentPresetBetSettingsCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "56E0854E61C3468580E491C38405580B",
            "STAG_GPI",
            "CNY",
            1,
            5000,
            20000,
            1);

        var response = new UpdateAgentPresetBetSettingsResponse()
        {
            IsAgentExist = false,
        };

        _ = fakeGameApiClient
             .UpdateAgentPresetBetSettingsAsync(
                Arg.Is(command.ApiUrl),
                Arg.Is(command.CompanyKey),
                Arg.Is(command.Username),
                Arg.Is(command.Min),
                Arg.Is(command.Max),
                Arg.Is(command.MaxPerMatch),
                Arg.Is(command.CasinoTableLimit))
             .Returns(response);

        var service = new AgentPresetBetSettingsCommandService(fakeGameApiClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        _ = fakeGameApiClient
             .Received(1)
             .RegisterAgentAsync(
                Arg.Is(command.ApiUrl),
                Arg.Is(command.CompanyKey),
                Arg.Is(command.Username),
                Arg.Is(command.Currency),
                Arg.Is(command.Min),
                Arg.Is(command.Max),
                Arg.Is(command.MaxPerMatch),
                Arg.Is(command.CasinoTableLimit));
    }
}
