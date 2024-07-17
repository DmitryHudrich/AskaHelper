using BebraHelper.Cli.Services;

internal class Aska {
    public static OsIdentity OsIdentity { get; } = OsIdentity.Identify();

    private PersistenceInfo[] PersistenceInfo() {
        var drives = HardDriveService.Drives;
        return drives
            .Select(drive => new PersistenceInfo() { Name = drive.Name, FreeSpace = drive.AvailableFreeSpace, })
            .ToArray();
    }

    public void PrintFetch() {
        Console.WriteLine("System:\t" + OsIdentity.FullDistroName);
        Console.WriteLine("Drive info:");
        foreach (var persistenceInfo in PersistenceInfo()) {
            Console.WriteLine("\tDisk name:\t\t" + persistenceInfo.Name);
            Console.WriteLine("\tAvailable space:\t" +
                              ((Double)persistenceInfo.FreeSpace / 1024 / 1024 / 1024).ToString("#.##"));
        }
    }
}
