namespace Tgs.GameInterface.SBOService.Core.Models;

public record LoginResponse
{
    public bool IsPlayerExist { get; init; }
    public string GameUrl { get; init; } = default!;
}
