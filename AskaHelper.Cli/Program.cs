using AskaHelper.Daemon;

String input;
if (args.Length > 0) {
    input = args[0];
    CommandHandler(input);
}
else {
    while (true) {
        Console.Write(">>> ");
        input = Console.ReadLine()!;
        CommandHandler(input);
    }
}

void CommandHandler(string s) {
    switch (s) {
        case "osinfo":
            OsInfo();
            break;
    }
}

static void OsInfo() {
    Console.WriteLine("""
                      ⣿⣿⣿⣿⣿⣿⡿⠟⠋⠉⠉⠉⠙⠛⢿⣿⣿⣿⣿⣿
                      ⣿⣿⡟⢩⣶⠂⠄⠄⣠⣶⣿⣯⣉⣷⣦⠈⣻⣿⣿⣿
                      ⣿⣿⣿⣄⠁⠄⠄⢸⡿⠟⠛⠉⠉⠉⠛⢧⠘⣿⣿⣿
                      ⣿⣿⣿⡿⠄⠄⠄⠄⢀⠄⣠⡄⠄⠄⠄⠄⠄⢹⣿⣿
                      ⣿⣿⣿⡇⠄⠄⠄⣸⡘⢴⣻⣧⣤⢀⣂⡀⠄⢸⣿⣿
                      ⣿⣿⣿⡇⠄⠘⢢⣿⣷⣼⣿⣿⣿⣮⣴⢃⣤⣿⣿⣿
                      ⣿⣿⡿⠄⣠⣄⣀⣙⣿⣿⣿⣿⣿⡿⠋⢸⡇⢹⣿⣿
                      ⣿⣿⡇⠰⣻⣿⣿⣿⠿⠮⠙⠿⠓⠛⠄⠄⠈⠄⢻⣿
                      ⣿⡟⠄⠄⠈⠙⠋⠄⠄⠄⠄⠁⠄⠄⠄⠄⠄⠄⢾⣿
                      ⡏⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⠄⠄⠄⠄⠄⠄⠈⣿
                      ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢹ 
                      """);
    Console.WriteLine("You system:\t" + AskaService.OsIdentity.FullDistroName);
    Console.WriteLine("Drives:");
    foreach (var driveInfo in AskaService.HardwareIdentity.Persistences) {
        Console.WriteLine("\tName:\t\t" + driveInfo.Name + "\tFree space:\t" +
                          (Double)driveInfo.AvailableFreeSpace / 1024 / 1024 / 1024);
    }

    Console.WriteLine();
}
