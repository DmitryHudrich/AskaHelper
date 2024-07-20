using System.Net.Http.Json;
using AskaHelper.Daemon;
using AskaHelper.Daemon.Services.OsInteraction;

while (true) {
    Console.Write(">>> ");
    var input = Console.ReadLine();
    switch (input) {
        case "osinfo":
            OsInfo();
            break;
        case "exit":
            Environment.Exit(0);
            break;
    };
}

static void OsInfo() {
    Console.WriteLine("You system:\t" + AskaService.OsIdentity.FullDistroName);
    Console.WriteLine("Drives:");
    foreach (var driveInfo in AskaService.HardwareIdentity.Persistences) {
        Console.WriteLine("\tName:\t\t" + driveInfo.Name);
        Console.WriteLine("\tFree space:\t" + (double)driveInfo.AvailableFreeSpace / 1024 / 1024 / 1024);
    }
    Console.WriteLine();
}
