using System;
using System.IO;
using SHA256 = System.Security.Cryptography.SHA256;

namespace VBM
{
    class Character
    {
        public readonly FileInfo Char;

        private Character(string charName, bool isGameChar)
        {
            Char = new FileInfo($"{(isGameChar ? Utility.GameDir : Utility.BackupDir)}characters/{charName}.fch");
        }

        public static Character FromBackupDir(string name)
        {
            return new Character(name, false);
        }
        public static Character FromGameDir(string name)
        {
            return new Character(name, true);
        }
        public string Hash()
        {
            if (!IsValid())
                return "";
            using var sha = SHA256.Create();
            using var f = Char.OpenRead();
            return Convert.ToBase64String(sha.ComputeHash(f));
        }
        public bool IsValid()
        {
            return Char.Exists;
        }
    }
}
