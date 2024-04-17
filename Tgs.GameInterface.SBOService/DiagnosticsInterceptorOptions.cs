using System.Diagnostics;

namespace Tgs.GameInterface.SBOService;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.SBOService.DiagnosticsInterceptor");
}
