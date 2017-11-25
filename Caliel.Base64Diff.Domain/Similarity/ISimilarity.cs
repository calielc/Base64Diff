using System.Collections.Generic;

namespace Caliel.Base64Diff.Domain.Similarity {
    public interface ISimilarity {
        ArraySimilarities Similarity { get; }
        IReadOnlyCollection<ArrayDifference> Diffs { get; }
    }
}