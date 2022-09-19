namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record AgentPresetBetSettingsCommand(
    Uri ApiUrl,
    string CompanyKey,
    string Username,
    string Currency,
    int Min,
    int Max,
    int MaxPerMatch,
    int CasinoTableLimit);
