using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Protos;
using Tgs.GameInterface.SBOService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Controllers;

[ApiController]
[Route("[controller]")]
public class SBOController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "currency": "CNY",
	///             "min": 1,
	///             "max": 5000,
	///             "maxPerMatch": 20000,
	///             "casinoTableLimit": 1
	///          }
	/// </remarks>
	[HttpPost("agent/bet/settings", Name = "AgentPresetBetSettings")]
	public ValueTask AgentPresetBetSettingsAsync(
		[FromServices] ICommandService<AgentPresetBetSettingsCommand> service,
		AgentPresetBetSettingsViewModel source
		)
	{
		var command = new AgentPresetBetSettingsCommand(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.Currency,
			source.Min,
			source.Max,
			source.MaxPerMatch,
			source.CasinoTableLimit);

		return service.ExecuteAsync(
			command: command,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd",
	///             "orderId": "W3100251169407373312"
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public async ValueTask<FundResultViewModel> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		FundViewModel source
		)
	{
		var query = new FundInQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.PlayerCode,
			source.OrderId,
			source.Amount);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			result.TxnId,
			result.Resp);
	}

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd",
	///             "orderId": "D3100246965624246272"
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public async ValueTask<FundResultViewModel> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		FundViewModel source
		)
	{
		var query = new FundOutQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.PlayerCode,
			source.OrderId,
			source.Amount);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			result.TxnId,
			result.Resp);
	}

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "orderId": "D3100246965624246272"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public async ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusViewModel source
		)
	{
		var query = new FundStatusQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.OrderId);

		return await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Games the URL by sports book asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd",
	///             "lang": "zh-cn",
	///             "isMobile": false
	///         }
	/// </remarks>
	[HttpPost("game/url/sportsbook", Name = "GameUrlBySportsBook")]
	public async ValueTask<GameUrlResultViewModel> GameUrlBySportsBookAsync(
		[FromServices] IGetService<GameUrlBySportsBookQuery, GameUrlResult> service,
		GameUrlBySportsBookViewModel source)
	{
		var query = new GameUrlBySportsBookQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.PlayerCode,
			source.Lang,
			source.IsMobile);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new GameUrlResultViewModel(
			result.IsPlayerExist,
			result.GameUrl);
	}

	/// <summary>
	/// Games the URL by virtual sports asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd",
	///             "lang": "zh-cn"
	///         }
	/// </remarks>
	[HttpPost("game/url/virtualsports", Name = "GameUrlByVirtualSports")]
	public async ValueTask<GameUrlResultViewModel> GameUrlByVirtualSportsAsync(
		[FromServices] IGetService<GameUrlByVirtualSportsQuery, GameUrlResult> service,
		GameUrlBySportsBookViewModel source)
	{
		var query = new GameUrlByVirtualSportsQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.PlayerCode,
			source.Lang);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new GameUrlResultViewModel(
			result.IsPlayerExist,
			result.GameUrl);
	}

	/// <summary>
	/// Games the wager by sports book asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "startTime": "2022-09-22T17:00:00+08:00",
	///             "endTime": "2022-09-22T17:30:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/wager/sportsbook", Name = "GameWagerBySportsBook")]
	public async IAsyncEnumerable<GameWagerResultViewModel> GameWagerBySportsBookAsync(
		[FromServices] IQueryService<GameWagerBySportsBookQuery, GameWagerResult> service,
		GameWagerViewModel source)
	{
		var query = new GameWagerBySportsBookQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.StartTime,
			source.EndTime);

		var gameWagers = await service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (gameWagers.Length == 0)
		{
			yield break;
		}

		foreach (var gameWager in gameWagers)
		{
			yield return new GameWagerResultViewModel(
				gameWager.PlayerCode,
				gameWager.ExternalWagerId,
				gameWager.ExternalGameId,
				(GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameWager.Status, true),
				gameWager.ExternalCreatedOn,
				gameWager.ExternalClosedOn,
				gameWager.Bet,
				gameWager.ValidBet,
				gameWager.PlayerWinLoss,
				gameWager.Turnover,
				gameWager.Settle,
				gameWager.AdditionalData);
		}
	}

	/// <summary>
	/// Games the wager by virtual sports asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "startTime": "2022-09-22T16:30:00+08:00",
	///             "endTime": "2022-09-22T17:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/wager/virtualsports", Name = "GameWagerByVirtualSports")]
	public async IAsyncEnumerable<GameWagerResultViewModel> GameWagerByVirtualSportsAsync(
		[FromServices] IQueryService<GameWagerByVirtualSportsQuery, GameWagerResult> service,
		GameWagerViewModel source)
	{
		var query = new GameWagerByVirtualSportsQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.StartTime,
			source.EndTime);

		var gameWagers = await service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (gameWagers.Length == 0)
		{
			yield break;
		}

		foreach (var gameWager in gameWagers)
		{
			yield return new GameWagerResultViewModel(
				gameWager.PlayerCode,
				gameWager.ExternalWagerId,
				gameWager.ExternalGameId,
				(GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameWager.Status, true),
				gameWager.ExternalCreatedOn,
				gameWager.ExternalClosedOn,
				gameWager.Bet,
				gameWager.ValidBet,
				gameWager.PlayerWinLoss,
				gameWager.Turnover,
				gameWager.Settle,
				gameWager.AdditionalData);
		}
	}

	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd"
	///         }
	/// </remarks>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		PlayerCreateViewModel source)
		=> service.ExecuteAsync(
			command: new PlayerCreateCommand(
				ApiUrl: source.ApiUrl,
				CompanyKey: source.CompanyKey,
				Agent: source.Agent,
				PlayerCode: source.PlayerCode),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
			[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
			PlayerBalanceViewModel source)
	{
		var query = new PlayerBalanceQuery(
			source.ApiUrl,
			source.CompanyKey,
			source.Agent,
			source.PlayerCode);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new PlayerBalanceResultViewModel(
			result.IsPlayerExist,
			result.Balance,
			result.FreeBalance,
			result.OutstandingBalance);
	}

	/// <summary>
	/// Players the kick asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://ex-api-demo-yy.568win.com",
	///             "companyKey": "49ED28A987624CA0A3F834C3B8A69AAB",
	///             "agent": "STAG_GPI_CNY",
	///             "playerCode": "4ucmqplxkuyd"
	///         }
	/// </remarks>
	[HttpPost("player/kick", Name = "PlayerKick")]
	public ValueTask PlayerKickAsync(
		[FromServices] ICommandService<PlayerKickCommand> service,
		PlayerKickViewModel source)
		=> service
		.ExecuteAsync(
			command: new PlayerKickCommand(
				ApiUrl: source.ApiUrl,
				CompanyKey: source.CompanyKey,
				Agent: source.Agent,
				PlayerCode: source.PlayerCode),
			cancellationToken: HttpContext.RequestAborted);
}
