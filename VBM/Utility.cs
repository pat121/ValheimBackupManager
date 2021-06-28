using System;
using System.IO;

namespace VBM
{
    static class Utility
    {
        private const string LinuxBackupDir = "/.ValheimBackups/";
        private const string WindowsBackupDir = "/AppData/LocalLow/ValheimBackups/";
        private const string LinuxGameDir = "/.config/unity3d/IronGate/Valheim/";
        private const string WindowsGameDir = "/AppData/LocalLow/IronGate/Valheim/";

        public static readonly string BackupDir;
        public static readonly string GameDir;
        
        static Utility()
        {
            var windows = OperatingSystem.IsWindows();
            BackupDir = GetUserProfile() + (windows ? WindowsBackupDir : LinuxBackupDir);
            
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

        public static void CheckOS()
        {
            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
                return;
            Console.WriteLine("This program is only intended for Windows and Linux.");
            Environment.Exit(0);
        }
        public static void Compare(Character game, Character backup)
        {
            if (game.Hash() == backup.Hash())
            {
                Console.WriteLine("The file contents are identical.");
                return;
            }

            if (game.Char.LastWriteTime == backup.Char.LastWriteTime)
            {
                Console.WriteLine("The file contents are not identical, but they were both last written at the same time.");
                return;
            }

            if (game.Char.LastWriteTime > backup.Char.LastWriteTime)
                Console.WriteLine("The character file stored in the game directory is newer.");
            else
                Console.WriteLine("The character file stored in the backups directory is newer.");

            Console.WriteLine($"Game directory copy last modified     {game.Char.LastWriteTime}");
            Console.WriteLine($"Backup directory copy last modified   {backup.Char.LastWriteTime}");
        }
        public static void Compare(World game, World backup)
        {
            if (game.HashDatabase() == backup.HashDatabase())
                Console.WriteLine("The save files are identical");
            else
            {
                if (game.Database.LastWriteTime == backup.Database.LastWriteTime)
                    Console.WriteLine("The save files were last modified at the same time, but they are not identical");
                else
                {
                    if (game.Database.LastWriteTime > backup.Database.LastWriteTime)
                        Console.WriteLine("The save file copy stored in the game directory is newer");
                    else
                        Console.WriteLine("The save file copy stored in the backups directory is newer");
                    Console.WriteLine($"Game directory copy last modified     {game.Database.LastWriteTime}");
                    Console.WriteLine($"Backup directory copy last modified   {backup.Database.LastWriteTime}");
                }
            }

            if (game.HashMetadata() == backup.HashMetadata())
                Console.WriteLine("The metadata files are identical");
            else
            {
                if (game.Metadata.LastWriteTime == backup.Metadata.LastWriteTime)
                    Console.WriteLine("The metadata files were last modified at the same time, but they are not identical");
                else
                {
                    if (game.Metadata.LastWriteTime > backup.Metadata.LastWriteTime)
                        Console.WriteLine("The metadata file copy stored in the game directory is newer");
                    else
                        Console.WriteLine("The meta file copy stored in the backups directory is newer");
                    Console.WriteLine($"Game directory copy last modified     {game.Metadata.LastWriteTime}");
                    Console.WriteLine($"Backup directory copy last modified   {backup.Metadata.LastWriteTime}");
                }
            }
        }
        public static bool Confirm(string prompt, ConsoleKey yes = ConsoleKey.Y)
        {
            Console.Write(prompt);
            Console.WriteLine($" (Press {yes} for yes, any other key for no)");
            return Console.ReadKey(true).Key == yes;
        }
        public static void Copy(Character source, Character dest)
        {
            File.Copy(source.Char.FullName, dest.Char.FullName, true);
        }
        public static void Copy(World source, World dest)
        {
            File.Copy(source.Database.FullName, dest.Database.FullName, true);
            File.Copy(source.Metadata.FullName, dest.Metadata.FullName, true);
        }
        private static string GetUserProfile()
        {
            return Environment.GetEnvironmentVariable(OperatingSystem.IsWindows() ? "USERPROFILE" : "HOME");
        }
        public static void Initialize()
        {
            // Since CreateDirectory() does nothing if it already exists, don't bother returning
            if (!Directory.Exists(BackupDir))
                Console.WriteLine("Performing first-time setup.");
            Directory.CreateDirectory($"{BackupDir}worlds");
            Directory.CreateDirectory($"{BackupDir}characters");
        }
        public static bool IsGameInstalled()
        {
            return Directory.Exists(GameDir);
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
            var helpFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "About.txt");
            if (File.Exists(helpFile))
                Console.Write(File.ReadAllText(helpFile));
            else
                Console.WriteLine("ERROR: About file not found.");
        }
    }
}
