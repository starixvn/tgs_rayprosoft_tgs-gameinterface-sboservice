using System.Diagnostics;
using System.Globalization;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Polly;
using Tgs.GameInterface.SBOService.Core.Exceptions;
using Tgs.GameInterface.SBOService.Core.Extensions;
using Tgs.GameInterface.SBOService.Core.JsonConverters;
using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly ILogger<GameApiClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly TimeZoneInfo _timeZone;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        HttpClient httpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
            Converters = {
                new SBODateTimeConverter()
            }
        };
    }

    public async ValueTask<bool> CheckTransactionStatusAsync(
        Uri apiUrl,
        string companyKey,
        string txnId,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/check-transaction-status.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "TxnId", txnId },
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "CheckTransactionStatus",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        return id is not 0
            ? throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString())
            : true;
    }

    public async ValueTask<TransactionResponse> DepositAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string txnId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/deposit.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", username },
            { "Amount", amount },
            { "TxnId", txnId },
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "Deposit",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        return id is not 0
            ? throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString())
            : new TransactionResponse(
                TxnId: jsonNode["txnId"]?.GetValue<string>() ?? string.Empty,
                Resp: jsonNode.ToJsonString());
    }

    public async IAsyncEnumerable<BetBySportsBookResponse> GetBetListByModifyDateAgentBySportsBookAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var startTimeEastern = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _timeZone);
        var endTimeEastern = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _timeZone);

        var url = new Uri(apiUrl, "/web-root/restricted/report/get-bet-list-by-modify-date.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", agent },
            { "Portfolio", "SportsBook" },
            { "StartDate", startTimeEastern.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { "EndDate", endTimeEastern.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { "Language","en" }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "GetBetListByModifyDate",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        if (id is not 0)
        {
            throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString());
        }

        var results = jsonNode["result"]!.AsArray();

        if (!results.Any())
        {
            yield break;
        }

        var betTransactions = results.Select(x => new BetBySportsBookResponse(
                ExternalWagerId: x!["refNo"]!.ToString(),
                PlayerCode: x["username"]!.ToString().Split("_")[1],
                SportsType: x["sportsType"]!.ToString(),
                Status: x["status"]!.ToString(),
                OrderTime: DateTimeOffset.Parse(x["orderTime"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                ModifyDate: DateTimeOffset.Parse(x["modifyDate"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                SettleTime: DateTimeOffset.Parse(x["settleTime"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                WinLostDate: DateTimeOffset.Parse(x["winLostDate"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                Bet: decimal.Parse(x["stake"]!.ToString()),
                ValidBet: decimal.Parse(x["actualStake"]!.ToString()),
                Turnover: decimal.Parse(x["turnover"]!.ToString()),
                PlayerWinLoss: decimal.Parse(x["winLost"]!.ToString()),
                AdditionalData: x.ToJsonString()
            )) ?? Enumerable.Empty<BetBySportsBookResponse>();

        foreach (var betTransaction in betTransactions)
        {
            yield return betTransaction;
        }
    }

    public async IAsyncEnumerable<BetByVirtualSportsResponse> GetBetListByModifyDateAgentByVirtualSportsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var startTimeEastern = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _timeZone);
        var endTimeEastern = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _timeZone);

        var url = new Uri(apiUrl, "/web-root/restricted/report/get-bet-list-by-modify-date.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", agent },
            { "Portfolio", "VirtualSports" },
            { "StartDate", startTimeEastern.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { "EndDate", endTimeEastern.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { "Language","en" }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "GetBetListByModifyDateByVirtualSports",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        if (id is not 0)
        {
            throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString());
        }

        var results = jsonNode["result"]!.AsArray();

        if (!results.Any())
        {
            yield break;
        }

        var betTransactions = results.Select(x => new BetByVirtualSportsResponse(
                ExternalWagerId: x!["refNo"]!.ToString(),
                PlayerCode: x["username"]!.ToString().Split("_")[1],
                ProductType: x["productType"]!.ToString(),
                Status: x["status"]!.ToString(),
                OrderTime: DateTimeOffset.Parse(x["orderTime"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                ModifyDate: DateTimeOffset.Parse(x["modifyDate"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                SettleTime: DateTimeOffset.Parse(x["settleTime"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                WinLostDate: DateTimeOffset.Parse(x["winLostDate"]!.ToString(), null, DateTimeStyles.AssumeUniversal),
                Bet: decimal.Parse(x["stake"]!.ToString()),
                ValidBet: decimal.Parse(x["actualStake"]!.ToString()),
                Turnover: decimal.Parse(x["turnover"]!.ToString()),
                PlayerWinLoss: decimal.Parse(x["winLost"]!.ToString()),
                AdditionalData: x.ToJsonString()
            )) ?? Enumerable.Empty<BetByVirtualSportsResponse>();

        foreach (var betTransaction in betTransactions)
        {
            yield return betTransaction;
        }
    }

    public async ValueTask<GetPlayerBalanceResponse> GetPlayerBalanceAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/get-player-balance.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", username }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "GetPlayerBalance",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        return id is not 0 and not 3303
            ? throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString())
            : new GetPlayerBalanceResponse()
            {
                IsPlayerExist = id == 0,
                Balance = jsonNode["balance"]?.GetValue<decimal>() ?? 0,
                Outstanding = jsonNode["outstanding"]?.GetValue<decimal>() ?? 0,
            };
    }

    public async ValueTask<LoginResponse> LoginAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string portfolio,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/login.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", username },
            { "Portfolio", portfolio }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "Login",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        return id is not 0 and not 3303
            ? throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString())
            : id is 3303
            ? new LoginResponse()
            {
                IsPlayerExist = false,
                GameUrl = string.Empty
            }
            : new LoginResponse()
            {
                IsPlayerExist = true,
                GameUrl = jsonNode["url"]?.GetValue<string>() ?? string.Empty
            };
    }

    public async ValueTask LogoutAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/logout.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", username }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "Logout",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        if (id is not 0 and not 3303)
        {
            throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString());
        }
    }

    public async ValueTask RegisterAgentAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string currency,
        int min,
        int max,
        int maxPerMatch,
        int casinoTableLimit,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/agent/register-agent.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", agent },
            { "Password", "TgsPassw0rd" },
            { "Currency", currency },
            { "Min", min },
            { "Max", max },
            { "MaxPerMatch", maxPerMatch },
            { "CasinoTableLimit", casinoTableLimit }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "RegisterAgent",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        if (id is not 0 and not 3303)
        {
            throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString());
        }
    }

    public async ValueTask RegisterPlayerAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string agent,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/register-player.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", username },
            { "Agent", agent }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "RegisterPlayer",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        if (id is not 0 and not 4103)
        {
            throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString());
        }
    }

    public async ValueTask<UpdateAgentPresetBetSettingsResponse> UpdateAgentPresetBetSettingsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        int min,
        int max,
        int maxPerMatch,
        int casinoTableLimit,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/agent/update-agent-preset-bet-settings.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", agent },
            { "Min", min },
            { "Max", max },
            { "MaxPerMatch", maxPerMatch },
            { "CasinoTableLimit", casinoTableLimit }
        };

        var jsonNode = await PostAsync(
                url,
                request,
                "RegisterAgent",
                cancellationToken)
            .ConfigureAwait(false);

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        return id is not 0 and not 4101 and not 4104
            ? throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString())
            : id is 4101 or 4104
            ? new UpdateAgentPresetBetSettingsResponse()
            {
                IsAgentExist = false
            }
            : new UpdateAgentPresetBetSettingsResponse()
            {
                IsAgentExist = true
            };
    }

    public async ValueTask<TransactionResponse> WithdrawAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string txnId,
        decimal amount,
        bool isFullAmount,
        CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, "/web-root/restricted/player/withdraw.aspx");

        JsonObject request = new JsonObject
        {
            { "CompanyKey", companyKey },
            { "Username", username },
            { "Amount", amount },
            { "TxnId", txnId },
            { "IsFullAmount", isFullAmount }
        };

        var retryPolicy = Policy
                .HandleResult<JsonNode>(r =>
                    r["error"]!["id"]!.GetValue<int>() == 9720)
                    .WaitAndRetryAsync(1,
                        retryAttempt => TimeSpan.FromSeconds(5),
                        (response, retryTime, context) => _logger.LogWarning("Response:{response}", JsonSerializer.Serialize(response)));

        var jsonNode = await retryPolicy.ExecuteAsync(async ()
            => await PostAsync(
                url,
                request,
                "Withdraw",
                cancellationToken)
            .ConfigureAwait(false));

        var id = jsonNode["error"]!["id"]!.GetValue<int>();

        return id is not 0
            ? throw new GameProvisioningErrorException(jsonNode["error"]!.ToJsonString())
            : new TransactionResponse(
                TxnId: jsonNode["txnId"]?.GetValue<string>() ?? string.Empty,
                Resp: jsonNode.ToJsonString());
    }

    private async ValueTask<JsonNode> PostAsync(
        Uri apiUrl,
        JsonObject request,
        string action,
        CancellationToken cancellationToken = default)
    {
        var logPrefix = $"{nameof(GameApiClient)}.{action}".ToLower();

        var sw = Stopwatch.StartNew();

        _ = request.TryAdd("ServerId", "game-sbo");

        var content = new StringContent(request.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json);
        var httpResponseMessage = await _httpClient
        .PostAsync(
            apiUrl,
            content,
            cancellationToken)
        .ConfigureAwait(false);

        sw.Stop();

        var requestMessage = await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        var responseMessage = default(string);
        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken: cancellationToken) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = requestMessage.NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{JsonSerializer.Serialize(apiLogEvent)}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
