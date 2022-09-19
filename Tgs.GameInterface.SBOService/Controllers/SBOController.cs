using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "username": "STAG_GPI",
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
            source.Username,
            source.Currency,
            source.Min,
            source.Max,
            source.MaxPerMatch,
            source.CasinoTableLimit);

        return service
                .ExecuteAsync(
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029",
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
            source.Username,
            source.Amount);

        var result = await service
                 .GetAsync(
                     query: query,
                     cancellationToken: HttpContext.RequestAborted)
                 .ConfigureAwait(false);

        return new FundResultViewModel(
            result.TxnId,
            result.Refno,
            result.Balance);
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029",
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
            source.Username,
            source.Amount);

        var result = await service
                 .GetAsync(
                     query: query,
                     cancellationToken: HttpContext.RequestAborted)
                 .ConfigureAwait(false);

        return new FundResultViewModel(
            result.TxnId,
            result.Refno,
            result.Balance);
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "txnId": "3076969062631477248"
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
            source.TxnId);

        return await service
                 .GetAsync(
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029",
    ///             "lang": "cn",
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
            source.Username,
            source.Lang,
            source.IsMobile);

        var result = await service
                 .GetAsync(
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029",
    ///             "lang": "cn"
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
            source.Username,
            source.Lang);

        var result = await service
                 .GetAsync(
                     query: query,
                     cancellationToken: HttpContext.RequestAborted)
                 .ConfigureAwait(false);

        return new GameUrlResultViewModel(
            result.IsPlayerExist,
            result.GameUrl);
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
        => service
            .ExecuteAsync(
               command: new PlayerCreateCommand(
                    source.ApiUrl,
                    source.CompanyKey,
                    source.Agent,
                    source.Username),
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029"
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
            source.Username);

        var result = await service
                 .GetAsync(
                     query: query,
                     cancellationToken: HttpContext.RequestAborted)
                 .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
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
    ///             "companyKey": "56E0854E61C3468580E491C38405580B",
    ///             "agent": "STAG_GPI",
    ///             "username": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/kick", Name = "PlayerKick")]
    public ValueTask PlayerKickAsync(
        [FromServices] ICommandService<PlayerKickCommand> service,
        PlayerKickViewModel source)
    => service
        .ExecuteAsync(
            command: new PlayerKickCommand(
                source.ApiUrl,
                source.CompanyKey,
                source.Agent,
                source.Username),
            cancellationToken: HttpContext.RequestAborted);
}
