using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
{
    private readonly IGameApiClient _client;

    public PlayerKickCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
        => _client
        .LogoutAsync(
                command.ApiUrl,
                command.CompanyKey,
                $"{command.Agent.Split('_')[0]}_{command.Username}",
                cancellationToken);
}
