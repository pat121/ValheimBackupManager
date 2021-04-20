using System.IO;

namespace VBM
{
    class Character
    {
        public readonly FileInfo Char;

        private Character(string charName, bool isGameChar)
        {
            Char = new FileInfo($"{(isGameChar ? Utility.GameDir : Utility.BackupDir)}characters{Utility.DirectorySep}{charName}.fch");
        }

        public static Character FromBackupDir(string name)
        {
            return new Character(name, false);
        }
        public static Character FromGameDir(string name)
        {
            return new Character(name, true);
        }
        public bool IsValid()
        {
            return Char.Exists;
        }
    }
}
