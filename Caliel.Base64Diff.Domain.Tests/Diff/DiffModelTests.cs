using Caliel.Base64Diff.Domain.Diff;
using Caliel.Base64Diff.Domain.Similarity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Caliel.Base64Diff.Domain.Tests.Diff {
    [TestClass]
    public sealed class DiffModelTests {
        private Mock<DiffService> _serviceMock;

        [TestInitialize]
        public void SetUp() {
            _serviceMock = new Mock<DiffService>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Should_build_instance() {
            const string id = "dasjdkasl";
            var model = new DiffModel(_serviceMock.Object, id);

            Assert.AreEqual(id, model.Id, nameof(model.Id));
            Assert.AreSame(_serviceMock.Object, model.Owner, nameof(model.Owner));
            Assert.IsNull(model.Left, nameof(model.Left));
            Assert.IsNull(model.Right, nameof(model.Right));
        }

        [TestMethod]
        public void Should_set_left_array() {
            const string id = "dasjdkasl";
            var model = new DiffModel(_serviceMock.Object, id);

            var expected = new byte[] { 1, 2, 3 };
            var actual = model.SetLeft(expected);

            Assert.AreSame(actual, model);
            Assert.AreSame(expected, model.Left, nameof(model.Left));
            Assert.IsNull(model.Right, nameof(model.Right));
        }

        [TestMethod]
        public void Should_set_right_array() {
            const string id = "dsgfasdf";
            var model = new DiffModel(_serviceMock.Object, id);

            var expected = new byte[] { 1, 2, 3 };
            var actual = model.SetRight(expected);

            Assert.AreSame(actual, model);
            Assert.AreSame(expected, model.Right, nameof(model.Right));
            Assert.IsNull(model.Left, nameof(model.Left));
        }

        [TestMethod]
        [DataRow(null, new byte[] { 1, 2 })]
        [DataRow(new byte[] { 1, 2 }, null)]
        [DataRow(null, null)]
        public void Should_return_null_when_no_left_or_side(byte[] left, byte[] right) {
            const string id = "dsgfdsfsdfasdf";
            var model = new DiffModel(_serviceMock.Object, id).SetLeft(left).SetRight(right);

            var actual = model.Similarity;

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Should_return_null_when_no_left_or_side() {
            const string id = "dsgfdsfsdfasdf";
            var left = new byte[] { 4, 5, 6 };
            var right = new byte[] { 3, 8, 7, 9 };
            var model = new DiffModel(_serviceMock.Object, id).SetLeft(left).SetRight(right);

            var actual = model.Similarity as BytesSimilarity;
            Assert.IsNotNull(actual);
            Assert.AreEqual(left, actual.Left, nameof(actual.Left));
            Assert.AreEqual(right, actual.Right, nameof(actual.Right));
        }

        [TestMethod]
        public void Should_update_read_data() {
            const string id = "dsgfdsfsdfasdf";
            var model = new DiffModel(_serviceMock.Object, id);

            var left = new byte[] { 4, 5, 6 };
            var right = new byte[] { 3, 8, 7, 9 };

            var actual = model.SetData(new DiffData {
                Left = left,
                Right = right
            });

            Assert.AreSame(actual, model);
            Assert.AreSame(left, model.Left, nameof(model.Left));
            Assert.AreSame(right, model.Right, nameof(model.Right));
        }

        [TestMethod]
        public void Should_save_data() {
            _serviceMock.Setup(mock => mock.Save(It.IsAny<string>(), It.IsAny<DiffData>()));

            const string id = "dsgfdsfsdfasdf";
            var left = new byte[] { 4, 5, 6 };
            var right = new byte[] { 3, 8, 7, 9 };
            var model = new DiffModel(_serviceMock.Object, id).SetLeft(left).SetRight(right);

            model.Save();

            _serviceMock.Verify(mock => mock.Save(It.IsAny<string>(), It.IsAny<DiffData>()), Times.Once());
            _serviceMock.Verify(mock => mock.Save(id, It.Is<DiffData>(actual => actual.Left == left && actual.Right == right)));
        }
    }
}
