﻿namespace Caliel.Base64Diff.Api.Bussiness {
    public struct ArrayDifference {
        public ArrayDifference(int offset, int length) {
            Offset = offset;
            Length = length;
        }

        public int Offset { get; }
        public int Length { get; }
    }
}