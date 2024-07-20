using AskaHelper.Daemon.Services.OsInteraction.OsModules.UnixDependent;
using AskaHelper.Daemon.Services.OsInteraction.OsModules.WindowsDependent;

namespace AskaHelper.Daemon.Services.OsInteraction;

public static class HardDriveService {
    public static DriveInfo[] Drives { get; } = AnalyzeDrives();

    private static DriveInfo[] AnalyzeDrives() {
        var res = Aska.OsIdentity.Family switch {
            OsFamily.Unix => UnixDriveService.Analyze(),
            OsFamily.Windows => WindowsDriveService.Analyze(),
            _ => throw new ArgumentException(),
        };
        return res;
    }
}
