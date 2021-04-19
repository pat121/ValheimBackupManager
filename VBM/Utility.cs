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
                GameDir = File.ReadAllText("path.txt");
            else
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
        public static void Initialize()
        {
            // Since CreateDirectory() does nothing if it already exists, don't bother returning
            if (!Directory.Exists(BackupDir))
                Console.WriteLine("Performing first-time setup.");
            Directory.CreateDirectory($"{BackupDir}worlds");
            Directory.CreateDirectory($"{BackupDir}characters");
        }

        private static string GetUserProfile()
        {
            return Environment.GetEnvironmentVariable(OperatingSystem.IsWindows() ? "USERPROFILE" : "USER");
        }
    }
}
