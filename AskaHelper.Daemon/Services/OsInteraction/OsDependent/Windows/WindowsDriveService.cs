using System.Diagnostics;

namespace AskaHelper.Daemon.Services.OsInteraction.OsModules.WindowsDependent;

internal static class WindowsDriveService {
    public static DriveInfo[] Analyze() {
        Debug.Assert(AskaService.OsIdentity.Family == OsFamily.Windows);
        return DriveInfo.GetDrives().Where(drive => drive.IsReady == true).ToArray();
    }
}
