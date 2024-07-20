using System.Diagnostics;
using AskaHelper.Daemon.Services.OsInteraction.OsIdentity;

namespace AskaHelper.Daemon.Services.OsInteraction.OsDependent.Windows;

internal static class WindowsDriveService {
    public static DriveInfo[] Analyze() {
        Debug.Assert(AskaService.OsIdentity.Family == OsFamily.Windows);
        return DriveInfo.GetDrives().Where(drive => drive.IsReady == true).ToArray();
    }
}
