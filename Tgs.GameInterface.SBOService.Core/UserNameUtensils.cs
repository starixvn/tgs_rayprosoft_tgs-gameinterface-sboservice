namespace Tgs.GameInterface.SBOService.Core;

internal class UserNameUtensils : IUserNameUtensils
{
    public string GetUserName(string agent, string playerCode)
        => $"{agent.Split('_')[0]}_{playerCode}";
}
