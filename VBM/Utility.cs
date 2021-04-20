using System;
using System.IO;

namespace VBM
{
    static class Utility
    {
        private const string LinuxBackupDir = "/ValheimBackups/";
        private const string WindowsBackupDir = @"\AppData\LocalLow\ValheimBackups\";
        private const string LinuxGameDir = "/.config/unity3d/IronGate/Valheim/";
        private const string WindowsGameDir = @"\AppData\LocalLow\IronGate\Valheim\";

        public static readonly string BackupDir;
        public static readonly string GameDir;
        public static readonly char DirectorySep;

        static Utility()
        {
            var windows = OperatingSystem.IsWindows();
            BackupDir = GetUserProfile() + (windows ? WindowsBackupDir : LinuxBackupDir);
            DirectorySep = windows ? '\\' : '/';

            if (File.Exists("path.txt"))
            {
                GameDir = File.ReadAllText("path.txt");
                if (Directory.Exists(GameDir))
                    return;
                File.Delete("path.txt");
                Console.WriteLine("Your custom game install path did not exist and has been reset to the default.");
            }
            GameDir = GetUserProfile() + (windows ? WindowsGameDir : LinuxGameDir);
        }

        public static void CheckGameInstalled()
        {
            if (Directory.Exists(GameDir))
                return;
            Console.WriteLine("Error: could not locate Valheim save data. Please ensure that you have the game installed, or if your save data is stored in a separate location, please specify that location with the command \"VBM setgamepath <path>\"");
            Environment.Exit(0);
        }
        public static void CheckOS()
        {
            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
                return;
            Console.WriteLine("This program is only intended for Windows and Linux.");
            Environment.Exit(0);
        }
        public static void Copy(Character source, Character dest)
        {
            try
            {
                File.Copy(source.Char.FullName, dest.Char.FullName, true);
            }
            catch (IOException e)
            {
                PrintErrorAndExit("An unexpected error occurred while copying files: " + e.Message);
            }
        }
        public static void Copy(World source, World dest)
        {
            try
            {
                File.Copy(source.Database.FullName, dest.Database.FullName, true);
                File.Copy(source.Metadata.FullName, dest.Metadata.FullName, true);
            }
            catch (IOException e)
            {
                PrintErrorAndExit("An unexpected error occurred while copying files: " + e.Message);
            }
        }
        private static string GetUserProfile()
        {
            return Environment.GetEnvironmentVariable(OperatingSystem.IsWindows() ? "USERPROFILE" : "USER");
        }
        public static void Initialize()
        {
            // Since CreateDirectory() does nothing if it already exists, don't bother returning
            if (!Directory.Exists(BackupDir))
                Console.WriteLine("Performing first-time setup.");
            Directory.CreateDirectory($"{BackupDir}worlds");
            Directory.CreateDirectory($"{BackupDir}characters");
        }
        public static void PrintErrorAndExit(string message)
        {
            Print(message, ConsoleColor.Red);
            Environment.Exit(0);
        }
        public static void Print(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
        public static void ShowHelp()
        {
            if (File.Exists("About.txt"))
                Console.Write(File.ReadAllText("About.txt"));
            else
                Console.WriteLine("ERROR: About file not found.");
        }
    }
}
