using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;


public class IPALog
{
    public void WriteLog(string action, string details)
    {
        string logEntry = $"{DateTime.Now} - {action}: {details}";

        try
        {
            File.AppendAllText("ipalogfile.txt", logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    public void ReadLog()
    {
        try
        {
            string[] logEntries = File.ReadAllLines("ipalogfile.txt");

            foreach (var entry in logEntries)
            {
                Console.WriteLine(entry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading log file: {ex.Message}");
        }
    }
    public void SearchLog(string searchTerm)
    {
        try
        {
            string[] logEntries = File.ReadAllLines("ipalogfile.txt");

            foreach (var entry in logEntries)
            {
                if (entry.Contains(searchTerm))
                {
                    Console.WriteLine(entry);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching in log file: {ex.Message}");
        }
    }
}

public class IPADiskInfo
{
    public void GatherInfo(IPALog logger)
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        logger.WriteLog("Executed method", "Disk Scan was executed");
        foreach (DriveInfo d in allDrives)
        {
            Console.WriteLine("Drive {0}", d.Name);
            Console.WriteLine("Drive type: {0}", d.DriveType);
            if (d.IsReady == true)
            {
                Console.WriteLine("Volume label: {0}", d.VolumeLabel);
                Console.WriteLine("File system: {0}", d.DriveFormat);
                Console.WriteLine(
                    "Available space to current user:{0, 15} bytes",
                    d.AvailableFreeSpace);

                Console.WriteLine(
                    "Total available space:{0, 15} bytes",
                    d.TotalFreeSpace);

                Console.WriteLine(
                    "Total size of drive:{0, 15} bytes ",
                    d.TotalSize);
            }
            Console.WriteLine("######################################");
        }
    }
}

public class IPAFileInfo
{
    public void FileInfo(string Path,IPALog logger) {
        var Info = new FileInfo(Path);
        if (Info.Exists == true)
        {
            Console.WriteLine(Info.Name);
            Console.WriteLine(Info.Extension);
            Console.WriteLine(Info.LastWriteTime);
            Console.WriteLine(Info.LastAccessTime);
            Console.WriteLine(Info.Directory);
            Console.WriteLine(Info.Length.ToString());
        }
        logger.WriteLog("User action", $"File {Path} was scanned for Info");
        Console.WriteLine("######################################");
    }
}

public class IPADirInfo
{
    public void DirInfo(string Path, IPALog logger)
    {
        var Info = new DirectoryInfo(Path);
        if (Info.Exists == true)
        {   
            Console.WriteLine(Info.Name);
            var Files = Info.GetFiles();
            var SuvDirs = Info.GetDirectories();
            int counter = 0;
            foreach(var file in Files)
            {
                counter++;
            }
            Console.WriteLine($"Amount of file - {counter}");
            counter = 0;
            foreach(var dir in SuvDirs)
            {
                counter++;
            }
            Console.WriteLine($"Amount of sub directories - {counter}");
            Console.WriteLine(Info.CreationTime);
            Console.WriteLine(Info.Parent);
        }
        logger.WriteLog("User action", $"Directory {Path} was scanned for Info");
        Console.WriteLine("######################################");
    }
}

public class IPAFileManager
{
    private IPALog logger;

    public IPAFileManager(IPALog logger)
    {
        this.logger = logger;
    }

    public void ManageFiles(string diskPath)
    {
        try
        {
            string inspectDir = Path.Combine(diskPath, "IPAInspect");
            string inspectFilePath = Path.Combine(inspectDir, "ipadirinfo.txt");
            string filesDir = Path.Combine(diskPath, "IPAFiles");


            Directory.CreateDirectory(inspectDir);


            string[] diskEntries = Directory.GetFileSystemEntries(diskPath);


            File.WriteAllLines(inspectFilePath, diskEntries);
            logger.WriteLog("File Management", $"Information saved to {inspectFilePath}");


            string copiedFilePath = Path.Combine(inspectDir, "copied_ipadirinfo.txt");
            File.Copy(inspectFilePath, copiedFilePath);
            logger.WriteLog("File Management", $"Copied file to {copiedFilePath}");


            File.Delete(inspectFilePath);
            logger.WriteLog("File Management", $"Deleted original file {inspectFilePath}");


            Directory.CreateDirectory(filesDir);

            string extensionToCopy = ".txt"; 
            foreach (var entry in diskEntries.Where(e => Path.GetExtension(e) == extensionToCopy))
            {
                File.Copy(entry, Path.Combine(filesDir, Path.GetFileName(entry)));
            }
            logger.WriteLog("File Management", $"Copied files with extension {extensionToCopy} to {filesDir}");


            string movedFilesDir = Path.Combine(inspectDir, "MovedFiles");
            Directory.Move(filesDir, movedFilesDir);
            logger.WriteLog("File Management", $"Moved {filesDir} to {movedFilesDir}");


            string archivePath = Path.Combine(inspectDir, "FilesArchive.zip");
            ZipFile.CreateFromDirectory(movedFilesDir, archivePath);
            logger.WriteLog("File Management", $"Created archive at {archivePath}");


            string extractDir = Path.Combine(inspectDir, "ExtractedFiles");
            ZipFile.ExtractToDirectory(archivePath, extractDir);
            logger.WriteLog("File Management", $"Extracted archive to {extractDir}");
        }
        catch (Exception ex)
        {
            logger.WriteLog("File Management Error", ex.Message);
        }
    }
}

class Program
{
    static void Main()
    {
        IPALog logger = new IPALog();
        logger.WriteLog("User Action", "Program is working");
        IPADiskInfo diskInfo = new IPADiskInfo();
        diskInfo.GatherInfo(logger);
        string kakashki = Console.ReadLine();
        IPAFileInfo iPAFileInfo = new IPAFileInfo();
        iPAFileInfo.FileInfo(kakashki, logger);
        string poo = @"D:\Ne_hihanki";
        IPADirInfo iPADirInfo = new IPADirInfo();
        iPADirInfo.DirInfo(poo, logger);
        string disk = @"C:\";
        IPAFileManager fileManager = new IPAFileManager(logger);
        fileManager.ManageFiles(disk);
        string currentHour = DateTime.Now.ToString("HH");
        logger.SearchLog(currentHour);
        logger.WriteLog("User Action", "Program stopped");
    }
}