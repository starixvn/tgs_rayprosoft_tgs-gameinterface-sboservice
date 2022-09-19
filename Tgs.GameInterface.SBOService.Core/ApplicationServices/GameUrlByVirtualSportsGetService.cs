using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameUrlByVirtualSportsGetService : IGetService<GameUrlByVirtualSportsQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;

    public GameUrlByVirtualSportsGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlResult> GetAsync(
        GameUrlByVirtualSportsQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = $"{query.Agent.Split('_')[0]}_{query.Username}";
        var response = await _client
                .LoginAsync(
                    query.ApiUrl,
                    query.CompanyKey,
                    username,
                    "VirtualSports",
                    cancellationToken)
                .ConfigureAwait(false);

        return !response.IsPlayerExist
            ? new GameUrlResult(
                IsPlayerExist: false,
                GameUrl: string.Empty)
            : new GameUrlResult(
            IsPlayerExist: true,
            GameUrl: $"https://{response.GameUrl}&lang={query.Lang}");
    }
}
