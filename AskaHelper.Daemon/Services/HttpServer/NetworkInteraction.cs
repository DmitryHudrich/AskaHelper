using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using AskaHelper.Daemon.Services.OsInteraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

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

        Bind("/ping", (_, _, _) => Task.FromResult("pong"));
        Bind("/ping",
            (_, _, _) =>
                Task.FromResult(
                    JsonConvert.SerializeObject(new { Data = "Just a post method for ping-check, so *pong*" })),
            Method.Post);

        Bind("/system/info", (_, _, _) => {
            var drives = HardDriveService.Drives;
            return Task.FromResult(JsonConvert.SerializeObject(new {
                System = Aska.OsIdentity.FullDistroName,
                Drives = drives
                    .Select(drive => new PersistenceInfo() { Name = drive.Name, FreeSpace = drive.AvailableFreeSpace, })
                    .ToArray(),
            }));
        });

        // --------------------------------------------------------------------
        BindAll();
        return Task.CompletedTask;
    }

    private void BindAll() {
        Task.Run(async () => {
            while (true) {
                var context = await server.GetContextAsync();
                foreach (var endpoint in endpoints) {
                    logger.LogInformation(context.Request.Url?.LocalPath);
                    if (context.Request.Url?.LocalPath == endpoint.Data && CompareMethod(context, endpoint)) {
                        await ConnectionPreHandle(context, endpoint);
                    }
                }
            }
        });
    }

    private static Boolean CompareMethod(HttpListenerContext context, Endpoint endpoint) {
        var castedMethod = context.Request.HttpMethod switch {
            "GET" => Method.Get,
            "POST" => Method.Post,
            "PUT" => Method.Put,
            "PATCH" => Method.Patch,
            "DELETE" => Method.Delete,
            _ => throw new ArgumentException("Why?"),
        };
        return castedMethod == endpoint.Method;
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

    private void Bind(String endpoint, NetworkRequestHandler handler, Method method = Method.Get) {
        endpoints.Add(new Endpoint(endpoint, handler, method));
    }

    private void StartServer() {
        logger.LogInformation("Server is started.");
        server.Prefixes.Add(Address);
        server.Start();
    }
}
