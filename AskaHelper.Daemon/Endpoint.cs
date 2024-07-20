using AskaHelper.Daemon;

internal class Endpoint(String endpoint, NetworkRequestHandler handler) {
    public String Data => endpoint;
    public NetworkRequestHandler Handler => handler;
}
