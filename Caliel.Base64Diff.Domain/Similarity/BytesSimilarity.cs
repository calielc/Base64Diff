using System.Collections.Generic;
using System.Linq;

namespace Caliel.Base64Diff.Domain.Similarity {
    public sealed class BytesSimilarity : ISimilarity {
        public BytesSimilarity(byte[] left, byte[] right) {
            Left = left;
            Right = right;

            if (left is null || right is null) {
                Similarity = ArraySimilarities.NotEqualSize;
            }
            else if (left.Length != right.Length) {
                Similarity = ArraySimilarities.NotEqualSize;
            }
            else if (left.SequenceEqual(right)) {
                Similarity = ArraySimilarities.AreEquals;
            }
            else {
                Similarity = ArraySimilarities.SameSize;
                Diffs = BuildDiffs().ToArray();
            }
        }

        public byte[] Left { get; }
        public byte[] Right { get; }

        public ArraySimilarities Similarity { get; }

        public IReadOnlyCollection<ArrayDifference> Diffs { get; }

        private IEnumerable<ArrayDifference> BuildDiffs() {
            var offset = 0;
            while (offset < Left.Length) {
                if (Left[offset] == Right[offset]) {
                    offset += 1;
                    continue;
                }

                var idx = offset + 1;
                while (idx < Left.Length) {
                    if (Left[idx] == Right[idx]) {
                        yield return new ArrayDifference(offset, idx - offset);

                        offset = idx;
                        break;
                    }

                    idx += 1;
                }
            }
        }
    }
}