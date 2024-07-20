namespace AskaHelper.Daemon.Services.HttpServer;

internal class Endpoint(String endpoint, NetworkRequestHandler handler, Method method = Method.Get) {
    public String Data => endpoint;
    public Method Method => method;
    public NetworkRequestHandler Handler => handler;
}
