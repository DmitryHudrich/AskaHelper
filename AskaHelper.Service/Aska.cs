using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using AskaHelper.Cli.Services;
using AskaHelper.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

await new Aska().Initialize();
Console.ReadLine();

internal class Aska {
    private ILogger<Aska> logger;

    public Aska() {
        var services = new ServiceCollection();
        Services = services.ConfigureServices().BuildServiceProvider();
        logger = Services.GetRequiredService<ILogger<Aska>>();
    }

    public static OsIdentity OsIdentity { get; } = OsIdentity.Identify();
    public IServiceProvider Services { get; set; }

    private PersistenceInfo[] PersistenceInfo() {
        var drives = HardDriveService.Drives;
        return drives
            .Select(drive => new PersistenceInfo() { Name = drive.Name, FreeSpace = drive.AvailableFreeSpace, })
            .ToArray();
    }

    public async Task Initialize() {
        using var serviceScope = Services.CreateScope();
        await Services.GetRequiredService<NetworkInteraction>().EndpointsPrepare();
    }
}
