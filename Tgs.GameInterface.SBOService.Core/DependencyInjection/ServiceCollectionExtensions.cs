using System.Reflection;
using Tgs.GameInterface.SBOService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameInterfaceCore(
		this IServiceCollection services)
		=> services
		.RegisterAllServiceUtilityTypes(
			targetAssembly: Assembly.GetExecutingAssembly(),
			lifetime: ServiceLifetime.Transient)
		.AddHttpClient<IGameApiClient, GameApiClient>()
		.Services
		.AddSingleton<IUserNameUtensils, UserNameUtensils>();
}
