using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameUrlByVirtualSportsGetService : IGetService<GameUrlByVirtualSportsQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public GameUrlByVirtualSportsGetService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public async ValueTask<GameUrlResult> GetAsync(
        GameUrlByVirtualSportsQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = _userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await _client.LoginAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            username: username,
            portfolio: "VirtualSports",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !response.IsPlayerExist
            ? new GameUrlResult(
                IsPlayerExist: false,
                GameUrl: string.Empty)
            : new GameUrlResult(
                IsPlayerExist: true,
                GameUrl: $"https:{response.GameUrl}&lang={query.Lang}");
    }
}
