using System.Diagnostics;

namespace AskaHelper.Daemon.Services.OsInteraction.OsDependent.Unix;

internal static class UnixDriveService {
    public static DriveInfo[] Analyze() {
        var mtab = File.ReadAllLines("/etc/mtab");
        return (from mountpoint in mtab
            where mountpoint.StartsWith("/dev/") && !mountpoint.StartsWith("/dev/loop")
            select new DriveInfo(mountpoint.Split(' ')[1])).ToArray();
    }
}
