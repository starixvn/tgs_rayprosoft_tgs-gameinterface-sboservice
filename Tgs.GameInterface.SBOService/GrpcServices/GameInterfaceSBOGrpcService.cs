using Grpc.Core;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.GameInterface.SBOService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.GrpcServices;

public class GameInterfaceSBOGrpcService : GameInterfaceSBOService.GameInterfaceSBOServiceBase
{
    private readonly ICommandService<AgentPresetBetSettingsCommand> _agentPresetBetSettingsService;
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly IGetService<GameUrlBySportsBookQuery, GameUrlResult> _gameUrlBySportsBookService;
    private readonly IGetService<GameUrlByVirtualSportsQuery, GameUrlResult> _gameUrlByVirtualSportsService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
    private readonly ICommandService<PlayerKickCommand> _playerKickService;

    public GameInterfaceSBOGrpcService(
        ICommandService<AgentPresetBetSettingsCommand> agentPresetBetSettingsService,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        IGetService<GameUrlBySportsBookQuery, GameUrlResult> gameUrlBySportsBookService,
        IGetService<GameUrlByVirtualSportsQuery, GameUrlResult> gameUrlByVirtualSportsService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        ICommandService<PlayerKickCommand> playerKickService
        )
    {
        _agentPresetBetSettingsService = agentPresetBetSettingsService ?? throw new ArgumentNullException(nameof(agentPresetBetSettingsService));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _gameUrlBySportsBookService = gameUrlBySportsBookService ?? throw new ArgumentNullException(nameof(gameUrlBySportsBookService));
        _gameUrlByVirtualSportsService = gameUrlByVirtualSportsService ?? throw new ArgumentNullException(nameof(gameUrlByVirtualSportsService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        _playerKickService = playerKickService ?? throw new ArgumentNullException(nameof(playerKickService));
    }

    public override async Task<BaseReply> AgentPresetBetSettings(
        AgentPresetBetSettingsRequest request,
        ServerCallContext context)
    {
        var command = new AgentPresetBetSettingsCommand(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Username,
            request.Currency,
            request.Min,
            request.Max,
            request.MaxPerMatch,
            request.CasinoTableLimit);

        await _agentPresetBetSettingsService
                .ExecuteAsync(
                    command: command,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<FundReply> FundIn(
        FundRequest request,
        ServerCallContext context)
    {
        var query = new FundInQuery(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username,
            request.Amount);

        var result = await _fundInService
                 .GetAsync(
                     query: query,
                     cancellationToken: context.CancellationToken)
                 .ConfigureAwait(false);

        return new FundReply()
        {
            TxnId = result.TxnId,
            Refno = result.Refno,
            Balance = result.Balance
        };
    }

    public override async Task<FundReply> FundOut(
        FundRequest request,
        ServerCallContext context)
    {
        var query = new FundOutQuery(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username,
            request.Amount);

        var result = await _fundOutService
                 .GetAsync(
                     query: query,
                     cancellationToken: context.CancellationToken)
                 .ConfigureAwait(false);

        return new FundReply()
        {
            TxnId = result.TxnId,
            Refno = result.Refno,
            Balance = result.Balance
        };
    }

    public override async Task<FundStatusReply> FundStatus(
        FundStatusRequest request,
        ServerCallContext context)
    {
        var query = new FundStatusQuery(
            new(request.ApiUrl),
            request.CompanyKey,
            request.TxnId);

        var result = await _fundStatusService
                 .GetAsync(
                     query: query,
                     cancellationToken: context.CancellationToken)
                 .ConfigureAwait(false);

        return new FundStatusReply()
        {
            IsOrderExist = result
        };
    }

    public override async Task<GameUrlReply> GameUrlBySportsBook(
        GameUrlBySportsBookRequest request,
        ServerCallContext context)
    {
        var query = new GameUrlBySportsBookQuery(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username,
            request.Lang,
            request.IsMobile);

        var result = await _gameUrlBySportsBookService
                 .GetAsync(
                     query: query,
                     cancellationToken: context.CancellationToken)
                 .ConfigureAwait(false);

        return new GameUrlReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            GameUrl = result.GameUrl
        };
    }

    public override async Task<GameUrlReply> GameUrlByVirtualSports(
        GameUrlByVirtualSportsRequest request,
        ServerCallContext context)
    {
        var query = new GameUrlByVirtualSportsQuery(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username,
            request.Lang);

        var result = await _gameUrlByVirtualSportsService
                 .GetAsync(
                     query: query,
                     cancellationToken: context.CancellationToken)
                 .ConfigureAwait(false);

        return new GameUrlReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            GameUrl = result.GameUrl
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(
        PlayerBalanceRequest request,
        ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username);

        var result = await _playerBalanceService
                 .GetAsync(
                     query: query,
                     cancellationToken: context.CancellationToken)
                 .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance
        };
    }

    public override async Task<BaseReply> PlayerCreate(
        PlayerCreateRequest request,
        ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username);

        await _playerCreateService
                .ExecuteAsync(
                     command: command)
                 .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<BaseReply> PlayerKick(
        PlayerKickRequest request,
        ServerCallContext context)
    {
        var command = new PlayerKickCommand(
            new(request.ApiUrl),
            request.CompanyKey,
            request.Agent,
            request.Username);

        await _playerKickService
                .ExecuteAsync(
                     command: command)
                 .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }
}
