namespace Tgs.GameInterface.SBOService.Core.Models;

public record TransactionResponse
{
    public string TxnId { get; init; } = default!;
    public string Refno { get; init; } = default!;
    public decimal Balance { get; init; }
    public decimal Outstanding { get; init; }
}
