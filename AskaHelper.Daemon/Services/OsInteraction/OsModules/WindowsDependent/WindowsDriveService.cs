using System.Diagnostics;

namespace AskaHelper.Cli.Services.OsModules.WindowsDependent;

internal static class WindowsDriveService {
    public static DriveInfo[] Analyze() {
        Debug.Assert(AskaBootstrap.OsIdentity.Family == OsFamily.Windows);
        return DriveInfo.GetDrives();
    }
}
