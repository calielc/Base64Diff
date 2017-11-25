using System.Collections.Generic;
using System.Linq;

namespace Caliel.Base64Diff.Api.Bussiness {
    public sealed class BytesSimilarity {
        public BytesSimilarity(byte[] left, byte[] right) {
            Left = left;
            Right = right;

            if (left.Length != right.Length) {
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

        public ArrayDifference[] Diffs { get; }

        private IEnumerable<ArrayDifference> BuildDiffs() {
            var offset = 0;
            while (offset < Left.Length) {
                if (Left[offset] == Right[offset]) {
                    offset += 1;
                    continue;
                }

                var length = 1;
                var idx = offset + 1;
                while (idx < Left.Length) {
                    if (Left[idx] == Right[idx]) {
                        yield return new ArrayDifference(offset, length);

                        offset = idx;
                        break;
                    }

                    length += 1;
                    idx += 1;
                }
            }
        }
    }
}