namespace Caliel.Base64Diff.Data.FileSystem {
    internal sealed class SystemIODirectory : ISystemIODirectory {
        public static readonly SystemIODirectory Instance = new SystemIODirectory();

        public bool Exists(string path) => System.IO.Directory.Exists(path);

        public void CreateDirectory(string path) => System.IO.Directory.CreateDirectory(path);
    }
}