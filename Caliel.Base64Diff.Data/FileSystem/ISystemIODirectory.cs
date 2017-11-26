namespace Caliel.Base64Diff.Data.FileSystem {
    public interface ISystemIODirectory {
        bool Exists(string path);
        void CreateDirectory(string path);
    }
}