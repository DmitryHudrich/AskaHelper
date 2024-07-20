namespace AskaHelper.Daemon.Services.HttpServer;

internal class Endpoint(String endpoint, NetworkRequestHandler handler) {
    public String Data => endpoint;
    public NetworkRequestHandler Handler => handler;
}
