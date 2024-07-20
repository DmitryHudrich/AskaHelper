namespace AskaHelper.Daemon.Services.OsInteraction.HardwareIdentity;

public class HardwareIdentity {
    public DriveInfo[] Persistences { get; } = HardDriveService.Drives;
}
