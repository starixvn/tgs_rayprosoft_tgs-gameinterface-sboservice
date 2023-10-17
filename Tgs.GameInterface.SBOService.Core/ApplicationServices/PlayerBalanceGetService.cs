using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public PlayerBalanceGetService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(
        PlayerBalanceQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = _userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await _client.GetPlayerBalanceAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            username: username,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult
        {
            IsPlayerExist = response.IsPlayerExist,
            Balance = response.Balance,
            OutstandingBalance = response.Outstanding
        };
    }
}
