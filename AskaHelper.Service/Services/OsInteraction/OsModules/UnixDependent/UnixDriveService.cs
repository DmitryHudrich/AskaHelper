using System.Diagnostics;

namespace AskaHelper.Cli.Services.OsModules.UnixDependent;

internal static class UnixDriveService {
    public static DriveInfo[] Analyze() {
        using var parseProcess = new Process();
        parseProcess.StartInfo =
            new ProcessStartInfo {
                UseShellExecute = false, FileName = "/bin/sh", Arguments = "./Scripts/get_mountpoints.sh", RedirectStandardOutput = true
            };
        parseProcess.Start();
        var res = GetDrives(parseProcess);
        return res.ToArray();
    }

    private static List<DriveInfo> GetDrives(Process parseProcess) {
        var res = new List<DriveInfo>();
        while (!parseProcess.StandardOutput.EndOfStream) {
            var line = parseProcess.StandardOutput.ReadLine();
            if (!String.IsNullOrWhiteSpace(line)) {
                res.Add(new DriveInfo(line));
            }
        }

        return res;
    }
}
