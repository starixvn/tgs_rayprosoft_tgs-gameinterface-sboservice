using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

internal class GameUrlBySportsBookGetService(
    IGameApiClient client,
    IUserNameUtensils userNameUtensils) : IGetService<GameUrlBySportsBookQuery, GameUrlResult>
{
    public async ValueTask<GameUrlResult> GetAsync(
        GameUrlBySportsBookQuery query,
        CancellationToken cancellationToken = default)
    {
        var username = userNameUtensils.GetUserName(query.Agent, query.PlayerCode);

        var response = await client.LoginAsync(
            apiUrl: query.ApiUrl,
            companyKey: query.CompanyKey,
            username: username,
            portfolio: "SportsBook",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsPlayerExist)
        {
            return new GameUrlResult(
                IsPlayerExist: false,
                GameUrl: string.Empty);
        }

        var device = query.IsMobile ? "m" : "d";

        return new GameUrlResult(
            IsPlayerExist: true,
            GameUrl: $"https:{response.GameUrl}&lang={query.Lang}&oddstyle={query.Oddstyle}&theme={query.Theme}&oddsmode={query.Oddstyle}&device={device}");
    }
}
