namespace Tgs.GameInterface.SBOService.Core.Exceptions;

public class GameProvisioningErrorException(string error) : Exception($"Game Provisioning Error Messages.({error})")
{
}
