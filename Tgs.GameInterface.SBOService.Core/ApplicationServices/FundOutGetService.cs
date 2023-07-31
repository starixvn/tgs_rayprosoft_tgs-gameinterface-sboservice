using Snowflake;
using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly SnowFlake _snowFlake;

    public FundOutGetService(
        IGameApiClient client,
        SnowFlake snowFlake)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _snowFlake = snowFlake ?? throw new ArgumentNullException(nameof(snowFlake));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var txnId = $"D{_snowFlake.NextId()}";
        var username = $"{query.Agent.Split('_')[0]}_{query.Username}";
        var response = await _client
            .DepositAsync(
                query.ApiUrl,
                query.CompanyKey,
                username,
                txnId,
                query.Amount,
                cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TxnId: response.TxnId,
            Resp: response.Resp);
    }
}
