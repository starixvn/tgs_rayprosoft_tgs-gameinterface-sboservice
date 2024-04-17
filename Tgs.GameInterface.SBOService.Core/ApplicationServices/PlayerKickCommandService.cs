using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class PlayerKickCommandService(
	IGameApiClient client,
	IUserNameUtensils userNameUtensils) : ICommandService<PlayerKickCommand>
{
	public ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
		=> client.LogoutAsync(
			apiUrl: command.ApiUrl,
			companyKey: command.CompanyKey,
			username: userNameUtensils.GetUserName(command.Agent, command.PlayerCode),
			cancellationToken: cancellationToken);
}
