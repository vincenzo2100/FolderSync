using System;
using System.IO;
using System.Threading;
using FolderSync;

if (args.Length != 4)
{
    Console.WriteLine("Usage: FolderSync <source> <replica> <intervalSeconds> <logPath>");
    return;
}
string basePath = Directory.GetCurrentDirectory();
string sourceDir = Path.Combine(basePath, args[0]);
string replicaDir = Path.Combine(basePath, args[1]);
if (!int.TryParse(args[2], out int intervalSeconds) || intervalSeconds <= 0)
{
    Console.WriteLine("Invalid interval. Must be a positive integer.");
    return;
}
string logPath = Path.Combine(basePath, args[3]);


var logger = new Logger(logPath);
var syncService = new DirectorySync(sourceDir, replicaDir, logger);


logger.Log($"Starting folder synchronization every {intervalSeconds} seconds.");
var syncTimer = new Timer(_ => syncService.Synchronize(), null, 0, intervalSeconds * 1000);

Console.WriteLine("Press Enter to exit...");
Console.ReadLine();