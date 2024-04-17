using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class FundInGetService(
	IGameApiClient client,
	IUserNameUtensils userNameUtensils) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var username = userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

		var response = await client.WithdrawAsync(
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
