using AskaHelper.Daemon.Services.OsInteraction;
using AskaHelper.Daemon.Services.OsInteraction.HardwareIdentity;
using AskaHelper.Daemon.Services.OsInteraction.OsIdentity;

namespace AskaHelper.Daemon;

internal static class AskaBootstrap {
    public static AskaService ConfigureAska() {
        return new AskaService();
    }
}

public class AskaService() {
    public static OsIdentity OsIdentity { get; } = OsIdentity.Identify();
    public static HardwareIdentity HardwareIdentity { get; } = new HardwareIdentity();
}
