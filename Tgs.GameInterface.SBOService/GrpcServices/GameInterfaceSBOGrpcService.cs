using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.SBOService.Core.ApplicationServices;
using Tgs.GameInterface.SBOService.Protos;
using Tgs.ServiceUtility;
using Enum = System.Enum;

namespace Tgs.GameInterface.SBOService.GrpcServices;

public class GameInterfaceSBOGrpcService(
    ILogger<GameInterfaceSBOGrpcService> logger,
    ICommandService<AgentPresetBetSettingsCommand> agentPresetBetSettingsService,
    IGetService<FundInQuery, FundResult> fundInService,
    IGetService<FundOutQuery, FundResult> fundOutService,
    IGetService<FundStatusQuery, bool> fundStatusService,
    IGetService<GameUrlBySportsBookQuery, GameUrlResult> gameUrlBySportsBookService,
    IGetService<GameUrlByVirtualSportsQuery, GameUrlResult> gameUrlByVirtualSportsService,
    IQueryService<GameWagerBySportsBookQuery, GameWagerResult> gameWagerBySportsBookService,
    IQueryService<GameWagerByVirtualSportsQuery, GameWagerResult> gameWagerByVirtualSportsService,
    IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
    ICommandService<PlayerCreateCommand> playerCreateService,
    ICommandService<PlayerKickCommand> playerKickService) : GameInterfaceSBO.GameInterfaceSBOBase
{
    public override Task<CheckingServiceReply> CheckingService(
        CheckingServiceRequest request,
        ServerCallContext context)
    {
        logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<BaseReply> AgentPresetBetSettings(
        AgentPresetBetSettingsRequest request,
        ServerCallContext context)
    {
        var command = new AgentPresetBetSettingsCommand(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            Currency: request.Currency,
            Min: request.Min,
            Max: request.Max,
            MaxPerMatch: request.MaxPerMatch,
            CasinoTableLimit: request.CasinoTableLimit);

        await agentPresetBetSettingsService.ExecuteAsync(
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
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode,
            OrderId: request.OrderId,
            Amount: request.Amount);

        var result = await fundInService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            TxnId = result.TxnId,
            Resp = result.Resp
        };
    }

    public override async Task<FundReply> FundOut(
        FundRequest request,
        ServerCallContext context)
    {
        var query = new FundOutQuery(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode,
            OrderId: request.OrderId,
            Amount: request.Amount);

        var result = await fundOutService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            TxnId = result.TxnId,
            Resp = result.Resp
        };
    }

    public override async Task<BaseReply> FundStatus(
        FundStatusRequest request,
        ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            OrderId: request.OrderId);

        var result = await fundStatusService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = result
        };
    }

    public override async Task<GameWagerReply> GameWagerBySportsBook(
        GameWagerRequest request,
        ServerCallContext context)
    {
        var query = new GameWagerBySportsBookQuery(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset());

        var gameWagers = await gameWagerBySportsBookService.QueryAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ToArrayAsync(
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameWagerReply();

        if (gameWagers.Length != 0)
        {
            foreach (var gameWager in gameWagers)
            {
                reply.Items.Add(new GameWagerInfo()
                {
                    PlayerCode = gameWager.PlayerCode,
                    ExternalWagerId = gameWager.ExternalWagerId,
                    ExternalGameId = gameWager.ExternalGameId,
                    Status = (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameWager.Status, true),
                    ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
                    Bet = gameWager.Bet,
                    ValidBet = gameWager.ValidBet,
                    PlayerWinLoss = gameWager.PlayerWinLoss,
                    Turnover = gameWager.Turnover,
                    Settle = gameWager.Settle,
                    AdditionalData = gameWager.AdditionalData
                });
            }
        }
        return reply;
    }

    public override async Task<GameWagerReply> GameWagerByVirtualSports(
        GameWagerRequest request,
        ServerCallContext context)
    {
        var query = new GameWagerByVirtualSportsQuery(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset());

        var gameWagers = await gameWagerByVirtualSportsService.QueryAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ToArrayAsync(
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameWagerReply();

        if (gameWagers.Length != 0)
        {
            foreach (var gameWager in gameWagers)
            {
                reply.Items.Add(new GameWagerInfo()
                {
                    PlayerCode = gameWager.PlayerCode,
                    ExternalWagerId = gameWager.ExternalWagerId,
                    ExternalGameId = gameWager.ExternalGameId,
                    Status = (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameWager.Status, true),
                    ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
                    Bet = gameWager.Bet,
                    ValidBet = gameWager.ValidBet,
                    PlayerWinLoss = gameWager.PlayerWinLoss,
                    Turnover = gameWager.Turnover,
                    Settle = gameWager.Settle,
                    AdditionalData = gameWager.AdditionalData
                });
            }
        }
        return reply;
    }

    public override async Task<GameUrlReply> GameUrlBySportsBook(
        GameUrlBySportsBookRequest request,
        ServerCallContext context)
    {
        var query = new GameUrlBySportsBookQuery(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode,
            Lang: request.Lang,
            IsMobile: request.IsMobile);

        var result = await gameUrlBySportsBookService.GetAsync(
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
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode,
            Lang: request.Lang);

        var result = await gameUrlByVirtualSportsService.GetAsync(
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
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode);

        var result = await playerBalanceService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance,
            FreeBalance = result.FreeBalance,
            OutstandingBalance = result.OutstandingBalance
        };
    }

    public override async Task<BaseReply> PlayerCreate(
        PlayerCreateRequest request,
        ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode);

        await playerCreateService.ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
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
            ApiUrl: new(request.ApiUrl),
            CompanyKey: request.CompanyKey,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode);

        await playerKickService.ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }
}
