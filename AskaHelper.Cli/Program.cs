namespace AskaHelper.Cli;

public static class Program {
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main() {
        var response = await httpClient.GetAsync("http://127.0.0.1:8888/ping");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
    }
}
