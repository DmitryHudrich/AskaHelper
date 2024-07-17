using System.Diagnostics;

namespace BebraHelper.Cli.Services.OsModules.WindowsDependent;

internal static class WindowsDriveService {
    public static DriveInfo[] Analyze() {
        Debug.Assert(Aska.OsIdentity.Family == OsFamily.Windows);
        return DriveInfo.GetDrives();
    }
}
