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
        var response = await _client
                .UpdateAgentPresetBetSettingsAsync(
                    command.ApiUrl,
                    command.CompanyKey,
                    command.Username,
                    command.Min,
                    command.Max,
                    command.MaxPerMatch,
                    command.CasinoTableLimit,
                    cancellationToken)
                .ConfigureAwait(false);

        if (!response.IsAgentExist)
        {
            await _client
                .RegisterAgentAsync(
                    command.ApiUrl,
                    command.CompanyKey,
                    command.Username,
                    command.Currency,
                    command.Min,
                    command.Max,
                    command.MaxPerMatch,
                    command.CasinoTableLimit,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
