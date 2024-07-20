using AskaHelper.Daemon;
using AskaHelper.Daemon.Services.HttpServer;
using AskaHelper.Daemon.Services.OsInteraction;
using Microsoft.Extensions.DependencyInjection;

var aska = AskaBootstrap.ConfigureAska();
aska.StartDaemons();

Console.ReadLine();

namespace AskaHelper.Daemon {
    internal static class AskaBootstrap {
        private static IServiceProvider? services;

        public static Aska ConfigureAska() {
            var servicesCollection = new ServiceCollection();
            Services = servicesCollection.ConfigureServices().BuildServiceProvider();
            return Services.GetRequiredService<Aska>();
        }

        public static IServiceProvider Services {
            get => services ?? throw new ArgumentNullException(nameof(Services), "Services isn't configured.");
            private set => services = value;
        }
    }

    internal class Aska(NetworkInteraction networkInteraction) {
        public static OsIdentity OsIdentity { get; } = OsIdentity.Identify();

        public void StartDaemons() {
            networkInteraction.EndpointsPrepare();
        }
    }
}
