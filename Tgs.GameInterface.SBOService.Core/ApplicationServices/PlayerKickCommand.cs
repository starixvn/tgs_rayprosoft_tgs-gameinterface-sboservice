namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record PlayerKickCommand(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    string Username);
