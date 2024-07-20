namespace AskaHelper.Daemon.Exceptions;

internal class UnsupportedOsException : Exception {
    public UnsupportedOsException() {
    }

    public UnsupportedOsException(String? message) : base(message) {
    }

    public UnsupportedOsException(String? message, Exception? innerException) : base(message, innerException) {
    }
}
