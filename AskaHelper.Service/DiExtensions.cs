using AskaHelper.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class DiExtensions {
    public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection) {
        serviceCollection.AddLogging(builder => builder.AddConsole());
        serviceCollection.AddSingleton<NetworkInteraction>();
        return serviceCollection;
    }
}