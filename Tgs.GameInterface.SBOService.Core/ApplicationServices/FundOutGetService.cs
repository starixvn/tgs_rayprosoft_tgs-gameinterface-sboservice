using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public FundOutGetService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var username = _userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await _client.DepositAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            username: username,
            txnId: query.OrderId,
            amount: query.Amount,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TxnId: response.TxnId,
            Resp: response.Resp);
    }
}
