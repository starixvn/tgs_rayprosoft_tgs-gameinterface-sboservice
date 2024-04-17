using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SBOService.Core.Exceptions;

namespace Tgs.GameInterface.SBOService.Core.UnitTest;

public class CheckTransactionStatusUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
		var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
		var txnId = Guid.NewGuid().ToString("N");

		var url = new Uri(apiUrl, "/web-root/restricted/player/check-transaction-status.aspx");

		var handler = new MockHttpMessageHandler();
		_ = handler
				.Expect(HttpMethod.Post, url.ToString())
				.Respond(
					HttpStatusCode.OK,
					new StringContent("{\"amount\":8947,\"txnId\":\"W0128012380\",\"refno\":\"" + txnId + "\",\"transactionType\":\"withdraw\",\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":0,\"msg\":\"No Error\"}}"));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.CheckTransactionStatusAsync(
			apiUrl: apiUrl,
			companyKey: companyKey,
			txnId: txnId);

		Assert.True(actual);
	}

	[Fact]
	public async Task Trade_Order_Does_Not_Exist()
	{
		var apiUrl = new Uri("https://ex-api-demo-yy.568win.com/");
		var companyKey = "49ED28A987624CA0A3F834C3B8A69AAB";
		var txnId = Guid.NewGuid().ToString("N");

		var url = new Uri(apiUrl, "/web-root/restricted/player/check-transaction-status.aspx");

		var handler = new MockHttpMessageHandler();
		_ = handler
				.Expect(HttpMethod.Post, url.ToString())
				.Respond(
					HttpStatusCode.OK,
					new StringContent("{\"serverId\":\"YY-demoanduat\",\"error\":{\"id\":4401,\"msg\":\"Invalid Transaction Id\"}}"));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<GameProvisioningErrorException>(()
			=> client.CheckTransactionStatusAsync(
				apiUrl: apiUrl,
				companyKey: companyKey,
				txnId: txnId).AsTask());
	}
}
