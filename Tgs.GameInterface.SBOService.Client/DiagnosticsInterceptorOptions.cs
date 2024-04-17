using System.Diagnostics;

namespace Tgs.GameInterface.SBOService.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.SBOService.Client.DiagnosticsInterceptor");
}
