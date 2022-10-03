using System.Reflection;
using Snowflake;
using Tgs.GameInterface.SBOService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceCore(
        this IServiceCollection services)
        => services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient)
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services
            .AddScoped((sp) => new SnowFlake(datacenterId: 1, machineId: 1));
}
