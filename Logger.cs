using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderSync
{
    public class Logger
{
    private readonly string logFile;

    public Logger(string logFile)
    {
        this.logFile = logFile;
        Directory.CreateDirectory(Path.GetDirectoryName(logFile)!);
    }

    public void Log(string message)
    {
        string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        Console.WriteLine(logLine);
        File.AppendAllText(logFile, logLine + Environment.NewLine);
    }
}
}