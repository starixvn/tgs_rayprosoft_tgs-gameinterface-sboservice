using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class AgentPresetBetSettingsCommandService : ICommandService<AgentPresetBetSettingsCommand>
{
    private readonly IGameApiClient _client;

    public AgentPresetBetSettingsCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(AgentPresetBetSettingsCommand command, CancellationToken cancellationToken = default)
    {
        var response = await _client.UpdateAgentPresetBetSettingsAsync(
            apiUrl: command.ApiUrl,
            companyKey: command.CompanyKey,
            agent: command.Agent,
            min: command.Min,
            max: command.Max,
            maxPerMatch: command.MaxPerMatch,
            casinoTableLimit: command.CasinoTableLimit,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsAgentExist)
        {
            await _client.RegisterAgentAsync(
                apiUrl: command.ApiUrl,
                companyKey: command.CompanyKey,
                agent: command.Agent,
                currency: command.Currency,
                min: command.Min,
                max: command.Max,
                maxPerMatch: command.MaxPerMatch,
                casinoTableLimit: command.CasinoTableLimit,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
