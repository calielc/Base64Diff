using Caliel.Base64Diff.Domain.Similarity;

namespace Caliel.Base64Diff.Domain.Diff {
    public interface IDiffModel : IIdentifyable {
        byte[] Left { get; }
        byte[] Right { get; }
        ISimilarity Similarity { get; }
    }
}