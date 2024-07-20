using AskaHelper.Daemon.Services.HttpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AskaHelper.Daemon;

public static class DiExtensions {
    public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection) {
        serviceCollection.AddLogging(builder => builder.AddConsole());
        serviceCollection.AddSingleton<NetworkInteraction>();
        serviceCollection.AddScoped<ScopeService>();
        serviceCollection.AddSingleton<Aska>();
        return serviceCollection;
    }
}

public class ScopeService {
    private static int n = 0;
    public ScopeService(ILogger<ScopeService> logger) {
        logger.LogInformation("Scope created " + n++);
    }
}
