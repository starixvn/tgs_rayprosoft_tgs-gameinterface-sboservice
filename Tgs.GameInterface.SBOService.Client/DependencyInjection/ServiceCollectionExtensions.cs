using Microsoft.Extensions.Options;
using Tgs.GameInterface.SBOService.Client;
using Tgs.GameInterface.SBOService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceSBOServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceSBOClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceSBOClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceSBOService.GameInterfaceSBOServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceSBOClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceSBOClient, GameInterfaceSBOClient>();
}
