using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.GameInterface.SBOService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameHistoryByVirtualSportsQueryService : IQueryService<GameHistoryByVirtualSportsQuery, GameHistoryResult>
{
    private readonly IGameApiClient _client;

    public GameHistoryByVirtualSportsQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
        GameHistoryByVirtualSportsQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responses = await _client
                .GetBetListByModifyDateAgentByVirtualSportsAsync(
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
                x.Username.Split("_")[1],
                x.RefNo,
                x.ProductType,
                x.Status,
                x.OrderTime,
                x.SettleTime,
                x.Stake,
                x.ActualStake,
                x.Status switch
                {
                    var status when status.Equals("running", StringComparison.OrdinalIgnoreCase) => 0,
                    var status when status.Equals("lose", StringComparison.OrdinalIgnoreCase) => x.WinLost,
                    var status when status.Equals("void", StringComparison.OrdinalIgnoreCase) => 0,
                    var status when status.Equals("draw", StringComparison.OrdinalIgnoreCase) => 0,
                    _ => x.WinLost - x.ActualStake

                },
                x.Turnover,
                x.Status switch
                {
                    var status when status.Equals("running", StringComparison.OrdinalIgnoreCase) => 0,
                    var status when status.Equals("lose", StringComparison.OrdinalIgnoreCase) => 0,
                    var status when status.Equals("void", StringComparison.OrdinalIgnoreCase) => x.WinLost,
                    var status when status.Equals("draw", StringComparison.OrdinalIgnoreCase) => x.WinLost,
                    _ => x.WinLost

                },
                JsonSerializer.Serialize(x)))
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
