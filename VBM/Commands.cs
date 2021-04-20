using System;
using System.IO;

namespace VBM
{
    static class Commands
    {
        [Command("Backup")]
        public static Result Backup(string worldName)
        {
            var game = World.FromGameDir(worldName);
            var backup = World.FromBackupDir(worldName);

            if (!game.IsValid())
                return Result.Fail("One or more game files weren't found");

            if (backup.IsValid())
            {
                Console.WriteLine("That backup file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");

                Console.WriteLine($"{worldName}.fwl (game) last modified {game.Metadata.LastWriteTime}");
                Console.WriteLine($"{worldName}.db  (game) last modified {game.Database.LastWriteTime}");
                Console.WriteLine($"{worldName}.fwl (backup) last modified {backup.Metadata.LastWriteTime}");
                Console.WriteLine($"{worldName}.db  (backup) last modified {backup.Database.LastWriteTime}");

                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    return Result.Canceled;
            }

            try
            {
                Utility.Copy(game, backup);
            }
            catch (Exception e)
            {
                return Result.Fail("An unexpected error occurred: " + e.Message);
            }
            return Result.Succeed("World \"" + worldName + "\" backed up successfully");
        }
        [Command("Help")]
        public static Result Help()
        {
            Utility.ShowHelp();
            return Result.Succeed("");
        }
        [Command("Restore")]
        public static Result Restore(string worldName)
        {
            var backup = World.FromBackupDir(worldName);
            var game = World.FromGameDir(worldName);

            if (!backup.IsValid())
                return Result.Fail("One or more backup files weren't found");
            
            if (game.IsValid())
            {
                Console.WriteLine("That game file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");

                Console.WriteLine($"{worldName}.fwl (backup) last modified {backup.Metadata.LastWriteTime}");
                Console.WriteLine($"{worldName}.db  (backup) last modified {backup.Database.LastWriteTime}");
                Console.WriteLine($"{worldName}.fwl (game) last modified {game.Metadata.LastWriteTime}");
                Console.WriteLine($"{worldName}.db  (game) last modified {game.Database.LastWriteTime}");

                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    return Result.Canceled;
            }

            try
            {
                Utility.Copy(backup, game);
            }
            catch (Exception e)
            {
                return Result.Fail("An unexpected error occurred while copying files: " + e.Message);
            }
            return Result.Succeed("World \"" + worldName + "\" restored successfully");
        }
        [Command("SetGamePath")]
        public static Result SetGamePath(string newPath)
        {
            if (Directory.Exists(newPath))
            {
                File.WriteAllText("path.txt", newPath);
                return Result.Succeed("Valheim installation path updated");
            }
            return new Result("The given directory does not exist.", ResultType.Failure);
        }
    }
}
