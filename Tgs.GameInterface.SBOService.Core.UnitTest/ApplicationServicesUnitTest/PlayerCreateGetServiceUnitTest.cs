using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class PlayerCreateGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		var fakeUserNameUtensils = Substitute.For<IUserNameUtensils>();

		var command = new PlayerCreateCommand(
			new Uri("https://ex-api-demo-yy.568win.com/"),
			"49ED28A987624CA0A3F834C3B8A69AAB",
			"4ucmqplxkuyd",
			"STAG_GPI_CNY");

		var username = "STAG_4ucmqplxkuyd";
		_ = fakeUserNameUtensils.GetUserName(
			agent: Arg.Is(command.Agent),
			playerCode: Arg.Is(command.PlayerCode))
			.Returns(username);

		var service = new PlayerCreateCommandService(
			fakeGameApiClient,
			fakeUserNameUtensils);

		await service.ExecuteAsync(command);

		_ = fakeGameApiClient
			.Received(1)
			.RegisterPlayerAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			companyKey: Arg.Is(command.CompanyKey),
			username: Arg.Is(username),
			agent: Arg.Is(command.Agent));
	}
}
