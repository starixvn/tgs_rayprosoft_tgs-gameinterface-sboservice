using Snowflake;
using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly SnowFlake _snowFlake;

    public FundInGetService(
        IGameApiClient client,
        SnowFlake snowFlake)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _snowFlake = snowFlake;
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var txnId = $"W{_snowFlake.NextId()}";
        var username = $"{query.Agent.Split('_')[0]}_{query.Username}";
        var response = await _client
            .WithdrawAsync(
                query.ApiUrl,
                query.CompanyKey,
                username,
                txnId,
                query.Amount,
                false,
                cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            response.TxnId,
            response.Refno,
            response.Balance - response.Outstanding);
    }
}
