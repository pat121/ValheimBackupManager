using System;
using System.IO;

namespace VBM
{
    class Program
    {
        static void Main(string[] args)
        {
            Utility.CheckOS();             // make sure OS is Windows or Linux
            Utility.CheckGameInstalled();  // make sure the OS-specific game save directory exists
            Utility.Initialize();          // create backups directory if it doesn't already exist

            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            var command = args[0];
            var arguments = args.Length > 1 ? args[1..] : Array.Empty<string>();
            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "backup":
                    if (arguments.Length != 1)
                    {
                        Console.WriteLine("ERROR: command \"backup\" expects one argument");
                        return;
                    }
                    Backup(arguments[0]);
                    break;
                case "restore":
                    if (arguments.Length != 1)
                    {
                        Console.WriteLine("ERROR: command \"restore\" expects one argument");
                        return;
                    }
                    Restore(arguments[0]);
                    break;
                case "setgamepath":
                    if (arguments.Length != 1)
                    {
                        Console.WriteLine("ERROR: command \"setgamepath\" expects one argument");
                        return;
                    }
                    var path = arguments[0];
                    if (!Directory.Exists(path))
                    {
                        Console.WriteLine("ERROR: given directory does not exist. Shame on you!");
                        return;
                    }

                    File.WriteAllText("path.txt", Path.GetFullPath(path));
                    break;
                default:
                    Console.WriteLine($"ERROR: command {command} not recognized");
                    break;
            }
        }

        static void Backup(string world)
        {
            var game = World.FromGameDir(world);
            var backup = World.FromBackupDir(world);

            if (!game.IsValid())
            {
                Console.WriteLine("ERROR: one or more game files weren't found");
                return;
            }

            if (backup.IsValid())
            {
                Console.WriteLine("That backup file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");
                
                Console.WriteLine($"{world}.fwl (game) last modified {game.Metadata.LastWriteTime}");
                Console.WriteLine($"{world}.db  (game) last modified {game.Database.LastWriteTime}");
                Console.WriteLine($"{world}.fwl (backup) last modified {backup.Metadata.LastWriteTime}");
                Console.WriteLine($"{world}.db  (backup) last modified {backup.Database.LastWriteTime}");

                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                {
                    Console.WriteLine("Operation canceled.");
                    return;
                }
            }

            Copy(game, backup);
            Console.WriteLine("Operation success.");
        }
        static void Restore(string world)
        {
            var backup = World.FromGameDir(world);
            var game = World.FromGameDir(world);

            if (!backup.IsValid())
            {
                Console.WriteLine("ERROR: one or more backup files weren't found");
                return;
            }

            if (game.IsValid())
            {
                Console.WriteLine("That game file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");

                Console.WriteLine($"{world}.fwl (backup) last modified {backup.Metadata.LastWriteTime}");
                Console.WriteLine($"{world}.db  (backup) last modified {backup.Database.LastWriteTime}");
                Console.WriteLine($"{world}.fwl (game) last modified {game.Metadata.LastWriteTime}");
                Console.WriteLine($"{world}.db  (game) last modified {game.Database.LastWriteTime}");

                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                {
                    Console.WriteLine("Operation canceled.");
                    return;
                }
            }

            Copy(backup, game);
            Console.WriteLine("Operation success.");
        }
        static void Copy(World source, World dest)
        {
            try
            {
                File.Copy(source.Database.FullName, dest.Database.FullName, true);
                File.Copy(source.Metadata.FullName, dest.Metadata.FullName, true);
            }
            catch (IOException e)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An unexpected error occurred while copying files:");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = oldColor;
                Environment.Exit(0);
            }
        }
        static void ShowHelp()
        {
            if (File.Exists("About.txt"))
                Console.Write(File.ReadAllText("About.txt"));
            else
                Console.WriteLine("ERROR: About file not found.");
        }
    }
}
