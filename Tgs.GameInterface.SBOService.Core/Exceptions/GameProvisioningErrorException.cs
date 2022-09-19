namespace Tgs.GameInterface.SBOService.Core.Exceptions;

public class GameProvisioningErrorException : Exception
{
    public GameProvisioningErrorException(string error) : base($"Game Provisioning Error Messages.({error})")
    {
    }
}
