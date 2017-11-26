namespace Caliel.Base64Diff.Data.FileSystem {
    public interface ISystemIOPath {
        char[] GetInvalidFileNameChars();
        string Combine(string path1, string path2);
    }
}