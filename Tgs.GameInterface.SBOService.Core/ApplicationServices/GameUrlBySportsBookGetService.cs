using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameUrlBySportsBookGetService : IGetService<GameUrlBySportsBookQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public GameUrlBySportsBookGetService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public async ValueTask<GameUrlResult> GetAsync(
        GameUrlBySportsBookQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = _userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await _client.LoginAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            username: username,
            portfolio: "SportsBook",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsPlayerExist)
        {
            return new GameUrlResult(
                IsPlayerExist: false,
                GameUrl: string.Empty);
        }

        var device = query.IsMobile ? "m" : "d";

        return new GameUrlResult(
            IsPlayerExist: true,
            GameUrl: $"https:{response.GameUrl}&lang={query.Lang}&oddstyle={query.Oddstyle}&theme={query.Theme}&oddsmode={query.Oddstyle}&device={device}");
    }
}
