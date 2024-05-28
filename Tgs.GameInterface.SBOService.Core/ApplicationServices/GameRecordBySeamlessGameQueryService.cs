using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameRecordBySeamlessGameQueryService(IGameApiClient client) : IQueryService<GameRecordBySeamlessGameQuery, GameRecordBySeamlessGameResult>
{
	public async IAsyncEnumerable<GameRecordBySeamlessGameResult> QueryAsync(
		GameRecordBySeamlessGameQuery query,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var responses = await client.GetBetListByModifyDateAgentBySeamlessGameAsync(
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
			.Select(x => new GameRecordBySeamlessGameResult(
				PlayerCode: x.PlayerCode,
				ExternalRoundId: x.ExternalRoundId,
				ExternalWagerId: x.ExternalWagerId,
				ExternalGameId: x.GameType,
				ExternalCreatedOn: x.OrderTime,
				ExternalClosedOn: x.SettleTime,
				Status: x.Status,
				Bet: x.Bet,
				Settle: x.Status switch
				{
					var status when status.Equals("running", StringComparison.OrdinalIgnoreCase) => 0,
					var status when status.Equals("void", StringComparison.OrdinalIgnoreCase) => x.WinLoss,
					_ => x.WinLoss + x.Turnover
				},
				Turnover: x.Turnover,
				WinLoss: x.Status switch
				{
					var status when status.Equals("running", StringComparison.OrdinalIgnoreCase) => 0,
					var status when status.Equals("void", StringComparison.OrdinalIgnoreCase) => 0,
					_ => x.WinLoss
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
