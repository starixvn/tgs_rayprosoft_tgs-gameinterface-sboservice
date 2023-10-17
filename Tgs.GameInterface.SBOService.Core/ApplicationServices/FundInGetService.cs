using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly IUserNameUtensils _userNameUtensils;

    public FundInGetService(
        IGameApiClient client,
        IUserNameUtensils userNameUtensils)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userNameUtensils = userNameUtensils ?? throw new ArgumentNullException(nameof(userNameUtensils));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var username = _userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await _client.WithdrawAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            username: username,
            txnId: query.OrderId,
            amount: query.Amount,
            isFullAmount: false,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TxnId: response.TxnId,
            Resp: response.Resp);
    }
}
