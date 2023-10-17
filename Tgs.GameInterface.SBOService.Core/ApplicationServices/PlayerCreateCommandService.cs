using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public PlayerCreateCommandService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _client
        .RegisterPlayerAsync(
            apiUrl: command.ApiUrl,
            companyKey: command.CompanyKey,
            username: _userNameUtensils.GetUserName(command.Agent, command.PlayerCode),
            agent: command.Agent,
            cancellationToken: cancellationToken);
}
