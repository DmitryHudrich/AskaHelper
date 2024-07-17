using BebraHelper.Cli.Services.OsModules.UnixDependent;
using BebraHelper.Cli.Services.OsModules.WindowsDependent;

namespace BebraHelper.Cli.Services;

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
