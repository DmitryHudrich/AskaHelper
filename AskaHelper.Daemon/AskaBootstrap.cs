using AskaHelper.Cli.Services;
using AskaHelper.Daemon;
using Microsoft.Extensions.DependencyInjection;

var aska = AskaBootstrap.ConfigureAska();
aska.StartDaemons();

Console.ReadLine();

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

    private static PersistenceInfo[] PersistenceInfo() {
        var drives = HardDriveService.Drives;
        return drives
            .Select(drive => new PersistenceInfo() { Name = drive.Name, FreeSpace = drive.AvailableFreeSpace, })
            .ToArray();
    }

    public void StartDaemons() {
        networkInteraction.EndpointsPrepare();
    }
}
