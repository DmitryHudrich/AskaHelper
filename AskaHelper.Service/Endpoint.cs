using AskaHelper.Service;

internal class Endpoint(String endpoint, NetworkRequestHandler handler) {
    public String Data => endpoint;
    public NetworkRequestHandler Handler => handler;
}
