using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		var fakeUserNameUtensils = Substitute.For<IUserNameUtensils>();

		var query = new PlayerBalanceQuery(
			new Uri("https://ex-api-demo-yy.568win.com/"),
			"49ED28A987624CA0A3F834C3B8A69AAB",
			"STAG_GPI_CNY",
			"4ucmqplxkuyd");

		var username = "STAG_4ucmqplxkuyd";
		_ = fakeUserNameUtensils.GetUserName(
			agent: Arg.Is(query.Agent),
			playerCode: Arg.Is(query.PlayerCode))
			.Returns(username);

		var response = new GetPlayerBalanceResponse()
		{
			IsPlayerExist = true,
			Balance = 100,
			Outstanding = 1
		};

		_ = fakeGameApiClient.GetPlayerBalanceAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			companyKey: Arg.Is(query.CompanyKey),
			username: Arg.Is(username))
			.Returns(response);

		var service = new PlayerBalanceGetService(
			fakeGameApiClient,
			fakeUserNameUtensils);

		var actual = await service.GetAsync(query);

		Assert.True(actual.IsPlayerExist);
		Assert.Equal(100m, actual.Balance);
	}
}
