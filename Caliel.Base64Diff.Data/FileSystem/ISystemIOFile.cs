namespace Caliel.Base64Diff.Data.FileSystem {
    public interface ISystemIOFile {
        bool Exists(string path);

        byte[] ReadAllBytes(string path);

        void WriteAllBytes(string path, byte[] bytes);

        void Delete(string path);
    }
}