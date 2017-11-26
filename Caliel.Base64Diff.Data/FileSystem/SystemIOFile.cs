namespace Caliel.Base64Diff.Data.FileSystem {
    internal sealed class SystemIOFile : ISystemIOFile {
        public static readonly SystemIOFile Instance = new SystemIOFile();

        public bool Exists(string path) => System.IO.File.Exists(path);

        public byte[] ReadAllBytes(string path) => System.IO.File.ReadAllBytes(path);

        public void WriteAllBytes(string path, byte[] bytes) => System.IO.File.WriteAllBytes(path, bytes);

        public void Delete(string path) => System.IO.File.Delete(path);
    }
}