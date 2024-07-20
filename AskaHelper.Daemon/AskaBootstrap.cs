using AskaHelper.Cli.Services;
using AskaHelper.Daemon;
using Microsoft.Extensions.DependencyInjection;

var aska = AskaBootstrap.ConfigureAska();
await aska.InitializeAsync();

Console.ReadLine();

internal class AskaBootstrap {
    private AskaBootstrap() {  }

    static AskaBootstrap() {
        var services = new ServiceCollection();
        Services = services.ConfigureServices().BuildServiceProvider();
    }

    public static AskaBootstrap ConfigureAska() {
        return new AskaBootstrap();
    }

    public static OsIdentity OsIdentity { get; } = OsIdentity.Identify();
    public static IServiceProvider Services { get; set; }

    private PersistenceInfo[] PersistenceInfo() {
        var drives = HardDriveService.Drives;
        return drives
            .Select(drive => new PersistenceInfo() { Name = drive.Name, FreeSpace = drive.AvailableFreeSpace, })
            .ToArray();
    }

    public async Task InitializeAsync() {
        await Services.GetRequiredService<NetworkInteraction>().EndpointsPrepare();
    }
}


