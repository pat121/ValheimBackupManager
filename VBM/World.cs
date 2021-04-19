using System;
using System.IO;
using SHA256 = System.Security.Cryptography.SHA256;

namespace VBM
{
    class World
    {
        public readonly FileInfo Database;
        public readonly FileInfo Metadata;

        private World(string name, bool isGameWorld)
        {
            var root = $"{(isGameWorld ? Utility.GameDir : Utility.BackupDir)}worlds{Utility.DirectorySep}{name}.";
            var mdPath = $"{root}fwl";
            var dbPath = $"{root}db";
            Database = new FileInfo(dbPath);
            Metadata = new FileInfo(mdPath);
        }

        public static World FromGameDir(string world)
        {
            return new World(world, true);
        }
        public static World FromBackupDir(string world)
        {
            return new World(world, false);
        }
        public string HashDatabase()
        {
            if (!IsValid())
                return "";
            using var sha = SHA256.Create();
            using var s = Database.OpenRead();
            return Convert.ToBase64String(sha.ComputeHash(s));
        }
        public string HashMetadata()
        {
            if (!IsValid())
                return "";
            using var sha = SHA256.Create();
            using var s = Metadata.OpenRead();
            return Convert.ToBase64String(sha.ComputeHash(s));
        }
        public bool IsValid()
        {
            return Database.Exists && Metadata.Exists;
        }
    }
}
