namespace Caliel.Base64Diff.Domain.Diff {
    public interface IDiffService {
        DiffModel Load(string id);
        DiffModel Create(string id);
        DiffModel LoadOrCreate(string id);
    }
}