using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client,
	IUserNameUtensils userNameUtensils) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var username = userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

		var response = await client.DepositAsync(
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
