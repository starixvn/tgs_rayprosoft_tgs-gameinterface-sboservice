using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameHistoryBySportsBookQueryService : IQueryService<GameHistoryBySportsBookQuery, GameHistoryResult>
{
    private readonly IGameApiClient _client;

    public GameHistoryBySportsBookQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
        GameHistoryBySportsBookQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responses = await _client
                .GetBetListByModifyDateAgentBySportsBookAsync(
                    query.ApiUrl,
                    query.CompanyKey,
                    query.Agent,
                    query.StartTime,
                    query.EndTime,
                    cancellationToken)
                .ToArrayAsync(cancellationToken)
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
            .Select(x => new GameHistoryResult(
                PlayerCode: x.PlayerCode,
                ExternalRoundId: x.ExternalRoundId,
                ExternalGameId: x.SportsType,
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
