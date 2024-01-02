using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerBalanceGetService(
    IGameApiClient client,
    IUserNameUtensils userNameUtensils) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(
        PlayerBalanceQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await client.GetPlayerBalanceAsync(
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
