using System.Diagnostics;
using System.Runtime.InteropServices;
using AskaHelper.Cli.Exceptions;

namespace AskaHelper.Daemon;

internal class OsIdentity {
    private static String[]? osreleaseContent;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private OsIdentity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public OsFamily Family { get; private init; }
    public OsDistroBase Distro { get; private init; }
    public String DistroName { get; private init; }
    public String FullDistroName { get; private init; }

    public static OsIdentity Identify() {
        var osFamily = Environment.OSVersion.Platform == PlatformID.Unix ? OsFamily.Unix : OsFamily.Windows;
        return new OsIdentity {
            Family = osFamily,
            Distro = osFamily == OsFamily.Windows ? OsDistroBase.Win : CheckDistro(),
            DistroName = osFamily == OsFamily.Windows ? "Windows" : "Linux",
            FullDistroName = osFamily == OsFamily.Windows ? Environment.OSVersion.ToString() : CheckFullDistroName(),
        };
    }

    private static String CheckFullDistroName() {
        return TakeOsReleaseParameter("PRETTY_NAME");
    }

    private static OsDistroBase CheckDistro() {
        var osId = TakeOsReleaseParameter("ID", "ID_LIKE");
        var res = osId switch {
            "arch" => OsDistroBase.Pacman,
            "debian" => OsDistroBase.Debian,
            _ => throw new UnsupportedOsException("Distro " + osId + " isn't supported now.")
        };
        return res;
    }

    private static String TakeOsReleaseParameter(params String[] names) {
        Debug.Assert(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        osreleaseContent ??= File.ReadAllLines("/etc/os-release");

        var conditions = new List<Boolean>();
        foreach (var name in names) {
            var prow = osreleaseContent.FirstOrDefault(s => s.Contains(name + '='));
            if (prow != null) {
                return prow[(prow.IndexOf('=') + 1)..].Trim();
            }
        }

        throw new ArgumentException("/etc/os-release file doesn't contains 'ID=' or 'ID_LIKE' fields.");
    }
}
