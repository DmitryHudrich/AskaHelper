using AskaHelper.Cli.Services.OsModules.UnixDependent;
using AskaHelper.Cli.Services.OsModules.WindowsDependent;

namespace AskaHelper.Cli.Services;

public static class HardDriveService {
    public static DriveInfo[] Drives { get; } = AnalyzeDrives();

    private static DriveInfo[] AnalyzeDrives() {
        var res = AskaBootstrap.OsIdentity.Family switch {
            OsFamily.Unix => UnixDriveService.Analyze(),
            OsFamily.Windows => WindowsDriveService.Analyze(),
            _ => throw new ArgumentException(),
        };
        return res;
    }
}
