﻿using System;
using System.IO;

namespace VBM
{
    static class Commands
    {
        [Command("Backup")]
        public static Result Backup(string objectName, string objectType = "world")
        {
            if (objectType == "character")
            {
                var game = Character.FromGameDir(objectName);
                var backup = Character.FromBackupDir(objectName);

                if (!game.IsValid())
                    return Result.Fail("One or more game files weren't found");

                if (backup.IsValid())
                {
                    Console.WriteLine("That backup file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no");
                    // TODO: comparison
                    if (Console.ReadKey(true).Key != ConsoleKey.Y)
                        return Result.Canceled;
                }

                Utility.Copy(game, backup);
                return Result.Succeed($"Character \"{objectName}\" backed up successfully");
            }
            else if (objectType == "world")
            {
                var game = World.FromGameDir(objectName);
                var backup = World.FromBackupDir(objectName);

                if (!game.IsValid())
                    return Result.Fail("One or more game files weren't found");

                if (backup.IsValid())
                {
                    Console.WriteLine("That backup file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");

                    Console.WriteLine($"{objectName}.fwl (game) last modified {game.Metadata.LastWriteTime}");
                    Console.WriteLine($"{objectName}.db  (game) last modified {game.Database.LastWriteTime}");
                    Console.WriteLine($"{objectName}.fwl (backup) last modified {backup.Metadata.LastWriteTime}");
                    Console.WriteLine($"{objectName}.db  (backup) last modified {backup.Database.LastWriteTime}");

                    if (Console.ReadKey(true).Key != ConsoleKey.Y)
                        return Result.Canceled;
                }

                Utility.Copy(game, backup);
                return Result.Succeed($"World \"{objectName}\" backed up successfully");
            }
            return Result.Fail($"Invalid argument: {objectType}");
        }

        [Command("Help")]
        public static Result Help()
        {
            Utility.ShowHelp();
            return Result.Succeed("");
        }

        [Command("Restore")]
        public static Result Restore(string objectName, string objectType = "world")
        {
            if (objectType == "character")
            {
                var backup = Character.FromBackupDir(objectName);
                var game = Character.FromGameDir(objectName);

                if (!backup.IsValid())
                    return Result.Fail("One or more backup files weren't found");

                if (game.IsValid())
                {
                    Console.WriteLine("That game file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");
                    // TODO: comparison
                    if (Console.ReadKey(true).Key != ConsoleKey.Y)
                        return Result.Canceled;
                }

                Utility.Copy(backup, game);
                return Result.Succeed($"Character \"{objectName}\" restored successfully");
            }
            else if (objectType == "world")
            {
                var backup = World.FromBackupDir(objectName);
                var game = World.FromGameDir(objectName);

                if (!backup.IsValid())
                    return Result.Fail("One or more backup files weren't found");

                if (game.IsValid())
                {
                    Console.WriteLine("That game file already exists. Do you want to overwrite it? (Press Y for yes, any other key for no)");

                    Console.WriteLine($"{objectName}.fwl (backup) last modified {backup.Metadata.LastWriteTime}");
                    Console.WriteLine($"{objectName}.db  (backup) last modified {backup.Database.LastWriteTime}");
                    Console.WriteLine($"{objectName}.fwl (game) last modified {game.Metadata.LastWriteTime}");
                    Console.WriteLine($"{objectName}.db  (game) last modified {game.Database.LastWriteTime}");

                    if (Console.ReadKey(true).Key != ConsoleKey.Y)
                        return Result.Canceled;
                }

                Utility.Copy(backup, game);
                return Result.Succeed("World \"" + objectName + "\" restored successfully");
            }

            return Result.Fail($"Invalid argument: {objectType}");
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
