using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core.UnitTest.ApplicationServicesUnitTest;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();
        var fakeUserNameUtensils = Substitute.For<IUserNameUtensils>();

        var query = new FundInQuery(
            ApiUrl: new Uri("https://ex-api-demo-yy.568win.com/"),
            CompanyKey: "49ED28A987624CA0A3F834C3B8A69AAB",
            Agent: "STAG_GPI_CNY",
            PlayerCode: "4ucmqplxkuyd",
            OrderId: "W3100251169407373312",
            Amount: 1);

        var username = "STAG_4ucmqplxkuyd";
        _ = fakeUserNameUtensils.GetUserName(
            agent: Arg.Is(query.Agent),
            playerCode: Arg.Is(query.PlayerCode))
            .Returns(username);

        var response = new TransactionResponse(
            TxnId: "W3100251169407373312",
            Resp: new JsonObject { ["TxnId"] = "W3100251169407373312" }.ToJsonString());
        _ = fakeGameApiClient.WithdrawAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            companyKey: Arg.Is(query.CompanyKey),
            username: Arg.Is(username),
            txnId: Arg.Any<string>(),
            amount: Arg.Is(query.Amount),
            isFullAmount: Arg.Is(false),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(response);

        var service = new FundInGetService(
            fakeGameApiClient,
            fakeUserNameUtensils);

        var actual = await service.GetAsync(query);

        Assert.Equal("W3100251169407373312", actual.TxnId);
        Assert.Equal(new JsonObject { ["TxnId"] = "W3100251169407373312" }.ToJsonString(), actual.Resp);
    }
}
