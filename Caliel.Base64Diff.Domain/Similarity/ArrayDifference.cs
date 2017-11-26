namespace Caliel.Base64Diff.Domain.Similarity {
    public struct ArrayDifference {
        public ArrayDifference(int offset, int length) {
            Offset = offset;
            Length = length;
        }

        /// <summary>
        /// Index where difference starts
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// How many bytes difference owns
        /// </summary>
        public int Length { get; }
    }
}