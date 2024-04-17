using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerCreateCommandService(
	IGameApiClient client,
	IUserNameUtensils userNameUtensils) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> client
		.RegisterPlayerAsync(
			apiUrl: command.ApiUrl,
			companyKey: command.CompanyKey,
			username: userNameUtensils.GetUserName(command.Agent, command.PlayerCode),
			agent: command.Agent,
			cancellationToken: cancellationToken);
}
