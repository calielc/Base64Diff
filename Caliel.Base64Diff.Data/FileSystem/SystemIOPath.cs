namespace Caliel.Base64Diff.Data.FileSystem {
    public sealed class SystemIOPath : ISystemIOPath {
        public static readonly SystemIOPath Instance = new SystemIOPath();

        public char[] GetInvalidFileNameChars() => System.IO.Path.GetInvalidFileNameChars();

        public string Combine(string path1, string path2) => System.IO.Path.Combine(path1, path2);
    }
}