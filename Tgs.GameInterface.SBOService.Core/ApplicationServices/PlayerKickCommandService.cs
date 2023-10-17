using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public PlayerKickCommandService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
        => _client
        .LogoutAsync(
            apiUrl: command.ApiUrl,
            companyKey: command.CompanyKey,
            username: _userNameUtensils.GetUserName(command.Agent, command.PlayerCode),
            cancellationToken: cancellationToken);
}
