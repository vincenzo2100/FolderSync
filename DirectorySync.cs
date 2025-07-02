using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderSync
{
    public class DirectorySync
    {
        private readonly string _source, _replica;
        private readonly Logger _logger;
        public DirectorySync(string source, string replica, Logger logger)
        {
            _source = source;
            _replica = replica;
            _logger = logger;
        }

        public void Synchronize()
        {
            try
            {
                SyncDirectory(new DirectoryInfo(_source), new DirectoryInfo(_replica));
                _logger.Log("Synchronization completed.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {ex.Message}");
            }
        }
    
    private void SyncDirectory(DirectoryInfo source, DirectoryInfo replica)
    {
            if (!source.Exists) source.Create();
        if (!replica.Exists) replica.Create();

        foreach (var file in source.GetFiles())
        {
            string targetFilePath = Path.Combine(replica.FullName, file.Name);
            if (!File.Exists(targetFilePath) || file.LastWriteTimeUtc != File.GetLastWriteTimeUtc(targetFilePath))
            {
                file.CopyTo(targetFilePath, true);
                _logger.Log($"Copied: {file.FullName} -> {targetFilePath}");
            }
        }

        foreach (var dir in source.GetDirectories())
        {
            SyncDirectory(dir, new DirectoryInfo(Path.Combine(replica.FullName, dir.Name)));
        }

       
        foreach (var file in replica.GetFiles())
        {
            if (!File.Exists(Path.Combine(source.FullName, file.Name)))
            {
                file.Delete();
                _logger.Log($"Deleted file: {file.FullName}");
            }
        }

        foreach (var dir in replica.GetDirectories())
        {
            if (!Directory.Exists(Path.Combine(source.FullName, dir.Name)))
            {
                dir.Delete(true);
                _logger.Log($"Deleted directory: {dir.FullName}");
            }
        }
    }
    }
}