using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(
        PlayerBalanceQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = $"{query.Agent.Split('_')[0]}_{query.Username}";
        var response = await _client
            .GetPlayerBalanceAsync(
                query.ApiUrl,
                query.CompanyKey,
                username,
                cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult
        {
            IsPlayerExist = response.IsPlayerExist,
            Balance = response.Balance,
            OutstandingBalance = response.Outstanding

        };
    }
}
