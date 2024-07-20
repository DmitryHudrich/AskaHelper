using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AskaHelper.Daemon.Services.HttpServer;

internal delegate Task<String> NetworkRequestHandler(HttpListenerContext context, IServiceProvider services,
    String requestData);

internal class NetworkInteraction(ILogger<NetworkInteraction> logger) {
    private readonly HttpListener server = new HttpListener();
    private const String Address = "http://127.0.0.1:8888/";
    private readonly List<Endpoint> endpoints = [];

    public Task EndpointsPrepare() {
        StartServer();

        // Bind endpoints below 

        Bind("/health", async (context, services, requestData) => {
            services.GetRequiredService<ScopeService>();
            return requestData;
        });

        // --------------------------------------------------------------------
        BindAll();
        return Task.CompletedTask;
    }

    private void BindAll() {
        Task.Run(async () => {
            while (true) {
                foreach (var endpoint in endpoints) {
                    var context = await server.GetContextAsync();
                    logger.LogInformation(context.Request.Url?.LocalPath);
                    if (context.Request.Url != null && context.Request.Url.LocalPath == endpoint.Data) {
                        await ConnectionPreHandle(context, endpoint);
                    }
                }
            }
        });
    }

    private async Task ConnectionPreHandle(HttpListenerContext context, Endpoint endpoint) {
        using var serviceScope = AskaBootstrap.Services.CreateScope();
        var responseText = await ReadRequest(context, endpoint, serviceScope.ServiceProvider);
        await SendResponse(responseText, context);
    }

    private async Task<String> ReadRequest(
        HttpListenerContext context, Endpoint endpoint, IServiceProvider services) {
        var body = await new StreamReader(context.Request.InputStream).ReadToEndAsync();
        logger.LogInformation("Text: " + body);
        var responseText = await endpoint.Handler(context, services, body);
        return responseText;
    }

    private static async Task SendResponse(String responseText, HttpListenerContext context) {
        var buffer = Encoding.UTF8.GetBytes(responseText);
        context.Response.ContentLength64 = buffer.Length;
        await using var output = context.Response.OutputStream;
        await output.WriteAsync(buffer);
        await output.FlushAsync();
    }

    private void Bind(String endpoint, NetworkRequestHandler handler) {
        endpoints.Add(new Endpoint(endpoint, handler));
    }

    private void StartServer() {
        logger.LogInformation("Server is started.");
        server.Prefixes.Add(Address);
        server.Start();
    }
}
