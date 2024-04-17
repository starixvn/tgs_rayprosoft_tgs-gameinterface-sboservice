namespace Tgs.GameInterface.SBOService.Core.UnitTest;

public class UserNameUtensilsUnitTest
{
	[Fact]
	public void Normal_Process()
	{
		var utensils = new UserNameUtensils();

		var userName = utensils.GetUserName("STAG_GPI_CNY", "4ucmqplxkuyd");

		Assert.Equal("STAG_4ucmqplxkuyd", userName);
	}
}
