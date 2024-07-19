using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AskaHelper.Service;

internal delegate Task<String> NetworkRequestHandler(HttpListenerContext context, String requestData);

internal class NetworkInteraction(ILogger<NetworkInteraction> logger) {
    private readonly HttpListener server = new HttpListener();
    private const String Address = "http://127.0.0.1:8888/";
    private readonly List<Endpoint> endpoints = [];

    public Task EndpointsPrepare() {
        StartServer();
        
        // Bind endpoints below 
        
        Bind("/bebra", async (context, requestData) => requestData);
        
        // --------------------------------------------------------------------
        BindAll();
        return Task.CompletedTask;
    }

    private void BindAll() {
        var tasks = new List<Task<HttpListenerContext>>();
        foreach (var endpoint in endpoints) {
            server
                .GetContextAsync()
                .ContinueWith((async contextTask => await ConnectionPreHandle(contextTask, endpoint)));
        }
    }

    private async Task ConnectionPreHandle(Task<HttpListenerContext> contextTask, Endpoint endpoint) {
        await Task.Run(async () => {
            while (true) {
                var (context, responseText) = await ReadRequest(contextTask, endpoint);
                await SendResponse(responseText, context);
            }
        });
    }

    private async Task<(HttpListenerContext context, String responseText)> ReadRequest(Task<HttpListenerContext> contextTask, Endpoint endpoint)
    {
        var context = await contextTask;
        var body = await new StreamReader(context.Request.InputStream).ReadToEndAsync();
        logger.LogInformation("Text: " + body);
        var responseText = await endpoint.Handler(context, body);
        return (context, responseText);
    }

    private static async Task SendResponse(string responseText, HttpListenerContext context)
    {
        var buffer = Encoding.UTF8.GetBytes(responseText);
        context.Response.ContentLength64 = buffer.Length;
        await using var output = context.Response.OutputStream;
        await output.WriteAsync(buffer);
        await output.FlushAsync();
    }

    public void Bind(String endpoint, NetworkRequestHandler handler) {
        endpoints.Add(new Endpoint(endpoint, handler));
    }

    private void StartServer() {
        logger.LogInformation("Server is started.");
        server.Prefixes.Add(Address);
        server.Start();
    }
}
