namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    string PlayerCode);
