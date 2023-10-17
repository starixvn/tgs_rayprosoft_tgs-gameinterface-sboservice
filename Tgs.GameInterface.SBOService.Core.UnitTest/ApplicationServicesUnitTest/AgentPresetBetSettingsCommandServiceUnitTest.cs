using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class AgentPresetBetSettingsCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new AgentPresetBetSettingsCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
            "CNY",
            1,
            5000,
            20000,
            1);

        var response = new UpdateAgentPresetBetSettingsResponse()
        {
            IsAgentExist = true,
        };

        _ = fakeGameApiClient.UpdateAgentPresetBetSettingsAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            companyKey: Arg.Is(command.CompanyKey),
            agent: Arg.Is(command.Agent),
            min: Arg.Is(command.Min),
            max: Arg.Is(command.Max),
            maxPerMatch: Arg.Is(command.MaxPerMatch),
            casinoTableLimit: Arg.Is(command.CasinoTableLimit))
            .Returns(response);

        var service = new AgentPresetBetSettingsCommandService(fakeGameApiClient);

        await service.ExecuteAsync(command);

        _ = fakeGameApiClient
            .Received(0)
            .RegisterAgentAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            companyKey: Arg.Is(command.CompanyKey),
            agent: Arg.Is(command.Agent),
            currency: Arg.Is(command.Currency),
            min: Arg.Is(command.Min),
            max: Arg.Is(command.Max),
            maxPerMatch: Arg.Is(command.MaxPerMatch),
            casinoTableLimit: Arg.Is(command.CasinoTableLimit));
    }

    [Fact]
    public async Task Register_Agent()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new AgentPresetBetSettingsCommand(
            new Uri("https://ex-api-demo-yy.568win.com/"),
            "49ED28A987624CA0A3F834C3B8A69AAB",
            "STAG_GPI_CNY",
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
            apiUrl: Arg.Is(command.ApiUrl),
            companyKey: Arg.Is(command.CompanyKey),
            agent: Arg.Is(command.Agent),
            min: Arg.Is(command.Min),
            max: Arg.Is(command.Max),
            maxPerMatch: Arg.Is(command.MaxPerMatch),
            casinoTableLimit: Arg.Is(command.CasinoTableLimit))
            .Returns(response);

        var service = new AgentPresetBetSettingsCommandService(fakeGameApiClient);

        await service.ExecuteAsync(command);

        _ = fakeGameApiClient
            .Received(1)
            .RegisterAgentAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            companyKey: Arg.Is(command.CompanyKey),
            agent: Arg.Is(command.Agent),
            currency: Arg.Is(command.Currency),
            min: Arg.Is(command.Min),
            max: Arg.Is(command.Max),
            maxPerMatch: Arg.Is(command.MaxPerMatch),
            casinoTableLimit: Arg.Is(command.CasinoTableLimit));
    }
}
