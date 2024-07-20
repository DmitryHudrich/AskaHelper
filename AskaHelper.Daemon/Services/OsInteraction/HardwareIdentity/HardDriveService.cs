using AskaHelper.Daemon.Services.OsInteraction.OsDependent.Unix;
using AskaHelper.Daemon.Services.OsInteraction.OsDependent.Windows;
using AskaHelper.Daemon.Services.OsInteraction.OsIdentity;

namespace AskaHelper.Daemon.Services.OsInteraction.HardwareIdentity;

internal static class HardDriveService {
    public static DriveInfo[] Drives { get; } = AnalyzeDrives();

    private static DriveInfo[] AnalyzeDrives() {
        var res = AskaService.OsIdentity.Family switch {
            OsFamily.Unix => UnixDriveService.Analyze(),
            OsFamily.Windows => WindowsDriveService.Analyze(),
            _ => throw new ArgumentException(),
        };
        return res;
    }
}
