using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameWagerByVirtualSportsQueryService : IQueryService<GameWagerByVirtualSportsQuery, GameWagerResult>
{
    private readonly IGameApiClient _client;

    public GameWagerByVirtualSportsQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameWagerResult> QueryAsync(
        GameWagerByVirtualSportsQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responses = await _client.GetBetListByModifyDateAgentByVirtualSportsAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            agent: query.Agent,
            startTime: query.StartTime,
            endTime: query.EndTime,
            cancellationToken: cancellationToken)
        .ToArrayAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        /*
         * 您好，请见以下分类，谢谢您。
         *
         * 未结算：
         *  running
         *
         * 已结算：
         *  draw
         *  lose
         *  won
         *
         * 作废：
         *  void
         */
        var gameRecords = responses.Where(x =>
            x.Status.Equals("running", StringComparison.OrdinalIgnoreCase) ||
            x.Status.Equals("draw", StringComparison.OrdinalIgnoreCase) ||
            x.Status.Equals("lose", StringComparison.OrdinalIgnoreCase) ||
            x.Status.Equals("won", StringComparison.OrdinalIgnoreCase) ||
            x.Status.Equals("void", StringComparison.OrdinalIgnoreCase))
            .Select(x => new GameWagerResult(
                PlayerCode: x.PlayerCode,
                ExternalWagerId: x.ExternalWagerId,
                ExternalGameId: x.ProductType,
                Status: x.Status,
                ExternalCreatedOn: x.OrderTime,
                ExternalClosedOn: x.SettleTime,
                Bet: x.Bet,
                ValidBet: x.ValidBet,
                PlayerWinLoss: x.Status switch
                {
                    var status when status.Equals("running", StringComparison.OrdinalIgnoreCase) => 0,
                    var status when status.Equals("void", StringComparison.OrdinalIgnoreCase) => 0,
                    _ => x.PlayerWinLoss
                },
                Turnover: x.Turnover,
                Settle: x.Status switch
                {
                    var status when status.Equals("running", StringComparison.OrdinalIgnoreCase) => 0,
                    var status when status.Equals("void", StringComparison.OrdinalIgnoreCase) => x.PlayerWinLoss,
                    _ => x.PlayerWinLoss + x.ValidBet
                },
                AdditionalData: x.AdditionalData))
            .AsQueryable();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords.AsEnumerable())
        {
            yield return gameRecord;
        }
    }
}
