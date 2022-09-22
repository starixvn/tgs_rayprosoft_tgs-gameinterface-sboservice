using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameUrlBySportsBookGetService : IGetService<GameUrlBySportsBookQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;

    public GameUrlBySportsBookGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlResult> GetAsync(
        GameUrlBySportsBookQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = $"{query.Agent.Split('_')[0]}_{query.Username}";
        var response = await _client
                .LoginAsync(
                    query.ApiUrl,
                    query.CompanyKey,
                    username,
                    "SportsBook",
                    cancellationToken)
                .ConfigureAwait(false);

        if (!response.IsPlayerExist)
        {
            return new GameUrlResult(
                IsPlayerExist: false,
                GameUrl: string.Empty
                );
        }

        var device = query.IsMobile ? "m" : "d";

        return new GameUrlResult(
            IsPlayerExist: true,
            GameUrl: $"https:{response.GameUrl}&lang={query.Lang}&oddstyle={query.Oddstyle}&theme={query.Theme}&oddsmode={query.Oddstyle}&device={device}");
    }
}
