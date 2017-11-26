using System.Linq;
using System.Text;
using Caliel.Base64Diff.Domain.Similarity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliel.Base64Diff.Api.Tests {
    [TestClass]
    public sealed class BytesSimilarityTests {
        [TestMethod]
        [DataRow("Caliel", "Costa")]
        [DataRow("Costa", "Caliel")]
        public void Should_build_instante(string left, string right) {
            var leftArray = Encoding.UTF8.GetBytes(left);
            var rightArray = Encoding.UTF8.GetBytes(right);

            var similarity = new BytesSimilarity(leftArray, rightArray);

            Assert.AreEqual(leftArray, similarity.Left, nameof(similarity.Left));
            Assert.AreEqual(rightArray, similarity.Right, nameof(similarity.Right));
        }

        [TestMethod]
        [DataRow("Caliel")]
        [DataRow("costa")]
        public void Should_return_are_equals(string text) {
            var array = Encoding.UTF8.GetBytes(text);
            var similarity = new BytesSimilarity(array, array);

            Assert.AreEqual(ArraySimilarities.AreEquals, similarity.Similarity, nameof(similarity.Similarity));
            Assert.IsNull(similarity.Diffs, nameof(similarity.Diffs));
        }

        [TestMethod]
        [DataRow("Caliel", "Calie")]
        [DataRow("cost", "costa")]
        public void Should_return_are_not_equal_size(string left, string right) {
            var similarity = new BytesSimilarity(Encoding.UTF8.GetBytes(left), Encoding.UTF8.GetBytes(right));

            Assert.AreEqual(ArraySimilarities.NotEqualSize, similarity.Similarity, nameof(similarity.Similarity));
            Assert.IsNull(similarity.Diffs, nameof(similarity.Diffs));
        }

        [TestMethod]
        [DataRow("Caliel", null)]
        [DataRow(null, "costa")]
        [DataRow(null, null)]
        public void Should_return_are_not_equal_size_when_is_null(string left, string right) {
            var leftSide = left is null ? null : Encoding.UTF8.GetBytes(left);
            var rightSide = right is null ? null : Encoding.UTF8.GetBytes(right);
            var similarity = new BytesSimilarity(leftSide, rightSide);

            Assert.AreEqual(ArraySimilarities.NotEqualSize, similarity.Similarity, nameof(similarity.Similarity));
            Assert.IsNull(similarity.Diffs, nameof(similarity.Diffs));
        }

        [TestMethod]
        public void Should_return_simple_differences() {
            var expected = new[] {
                new ArrayDifference(2, 1)
            };

            var similarity = new BytesSimilarity(Encoding.UTF8.GetBytes("Caliel"), Encoding.UTF8.GetBytes("CaLiel"));

            Assert.AreEqual(ArraySimilarities.SameSize, similarity.Similarity, nameof(similarity.Similarity));
            CollectionAssert.AreEqual(expected, similarity.Diffs.ToArray(), nameof(similarity.Diffs));
        }

        [TestMethod]
        public void Should_return_complex_differences() {
            var expected = new[] {
                new ArrayDifference(7, 4),
                new ArrayDifference(13, 1),
                new ArrayDifference(17, 2),
            };

            var left = Encoding.UTF8.GetBytes("Caliel Lima da Costa");
            var right = Encoding.UTF8.GetBytes("Caliel lIMA dA CoSTa");
            var similarity = new BytesSimilarity(left, right);

            Assert.AreEqual(ArraySimilarities.SameSize, similarity.Similarity, nameof(similarity.Similarity));
            CollectionAssert.AreEqual(expected, similarity.Diffs.ToArray(), nameof(similarity.Diffs));
        }
    }
}