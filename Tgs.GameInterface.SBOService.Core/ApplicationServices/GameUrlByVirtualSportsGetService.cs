using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameUrlByVirtualSportsGetService(
	IGameApiClient client,
	IUserNameUtensils userNameUtensils) : IGetService<GameUrlByVirtualSportsQuery, GameUrlResult>
{
	public async ValueTask<GameUrlResult> GetAsync(
		GameUrlByVirtualSportsQuery query,
		CancellationToken cancellationToken = default)
	{
		var username = userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

		var response = await client.LoginAsync(
			apiUrl: query.ApiUrl,
			companyKey: query.CompanyKey,
			username: username,
			portfolio: "VirtualSports",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return !response.IsPlayerExist
			? new GameUrlResult(
				IsPlayerExist: false,
				GameUrl: string.Empty)
			: new GameUrlResult(
				IsPlayerExist: true,
				GameUrl: $"https:{response.GameUrl}&lang={query.Lang}");
	}
}
