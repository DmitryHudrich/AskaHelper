using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using AskaHelper.Cli.Services;
using AskaHelper.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var aska = Aska.ConfigureAska();
await aska.InitializeAsync();
Console.ReadLine();

internal class Aska {
    private Aska() {  }

    static Aska() {
        var services = new ServiceCollection();
        Services = services.ConfigureServices().BuildServiceProvider();
    }

    public static Aska ConfigureAska() {
        
        return new Aska();
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
