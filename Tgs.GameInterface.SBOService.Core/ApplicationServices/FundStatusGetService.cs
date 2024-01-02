using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(
        FundStatusQuery query,
        CancellationToken cancellationToken = default)
        => client.CheckTransactionStatusAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            txnId: query.OrderId,
            cancellationToken: cancellationToken);
}
