namespace Caliel.Base64Diff.Data {
    public interface IDiffContentRepository {
        DiffContent Read(string id);
        void Save(DiffContent content);
    }
}