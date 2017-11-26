using System;
using System.Diagnostics;
using Caliel.Base64Diff.Data;
using Caliel.Base64Diff.Domain.Similarity;

namespace Caliel.Base64Diff.Domain.Diff {
    public sealed class DiffModel : IDiffModel, IEquatable<DiffModel> {
        internal DiffModel(DiffService service, string id) {
            Debug.Assert(service != null);

            Owner = service;
            Id = id;
        }

        internal DiffService Owner { get; }

        public string Id { get; }

        public byte[] Left { get; private set; }
        public byte[] Right { get; private set; }

        public ISimilarity Similarity {
            get {
                if (Left == null || Right == null) {
                    return default;
                }
                return new BytesSimilarity(Left, Right);
            }
        }

        public DiffModel SetLeft(byte[] bytes) {
            Left = bytes;

            return this;
        }

        public DiffModel SetRight(byte[] bytes) {
            Right = bytes;

            return this;
        }

        public void Save() {
            Owner.Save(Id, new DiffContent(Id) {
                Left = Left,
                Right = Right
            });
        }

        internal DiffModel SetData(DiffContent diffContent) {
            Left = diffContent.Left;
            Right = diffContent.Right;

            return this;
        }

        public bool Equals(DiffModel other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Equals(Owner, other.Owner)
                && string.Equals(Id, other.Id)
                && Equals(Left, other.Left)
                && Equals(Right, other.Right);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj is DiffModel model && Equals(model);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (Owner != null ? Owner.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Left != null ? Left.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(DiffModel left, DiffModel right) => Equals(left, right);

        public static bool operator !=(DiffModel left, DiffModel right) => !Equals(left, right);
    }
}