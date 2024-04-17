using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class FundOutGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		var fakeUserNameUtensils = Substitute.For<IUserNameUtensils>();

		var query = new FundOutQuery(
			new Uri("https://ex-api-demo-yy.568win.com/"),
			"49ED28A987624CA0A3F834C3B8A69AAB",
			"STAG_GPI_CNY",
			"4ucmqplxkuyd",
			"D3100246965624246272",
			1);

		var username = "STAG_4ucmqplxkuyd";
		_ = fakeUserNameUtensils.GetUserName(
			agent: Arg.Is(query.Agent),
			playerCode: Arg.Is(query.PlayerCode))
			.Returns(username);

		var response = new TransactionResponse(
			TxnId: "D3100246965624246272",
			Resp: new JsonObject { ["TxnId"] = "D3100246965624246272" }.ToJsonString());
		_ = fakeGameApiClient.DepositAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			companyKey: Arg.Is(query.CompanyKey),
			username: Arg.Is(username),
			txnId: Arg.Any<string>(),
			amount: Arg.Is(query.Amount),
			cancellationToken: Arg.Any<CancellationToken>())
			 .Returns(response);

		var service = new FundOutGetService(
			fakeGameApiClient,
			fakeUserNameUtensils);

		var actual = await service.GetAsync(query);

		Assert.Equal("D3100246965624246272", actual.TxnId);
		Assert.Equal(new JsonObject { ["TxnId"] = "D3100246965624246272" }.ToJsonString(), actual.Resp);
	}
}
