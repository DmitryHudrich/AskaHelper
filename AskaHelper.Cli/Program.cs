using System.Net.Http.Json;

namespace AskaHelper.Cli;

public static class Program {
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main() {
        var response = await httpClient.GetAsync("http://127.0.0.1:8888/system/info");
        var content = await response.Content.ReadFromJsonAsync<RootObject>();
        Console.WriteLine("System: " + content?.System);
        Console.WriteLine("Drives info: ");
        foreach (var drive in content.Drives) {
            Console.WriteLine("\tDrive name:\t" + drive.Name);
            Console.WriteLine("\tFree space:\t" + drive.FreeSpace / 1024 / 1024);
        }
    }
}

public class RootObject {
    public String System { get; set; }
    public Drives[] Drives { get; set; }
}

public class Drives {
    public String Name { get; set; }
    public Int64 FreeSpace { get; set; }
}
