using System;
using Caliel.Base64Diff.Data;
using Caliel.Base64Diff.Domain.Diff;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Caliel.Base64Diff.Domain.Tests.Diff {
    [TestClass]
    public sealed class DiffServiceTests {
        private DiffService _service;
        private Mock<IDiffContentRepository> _repositoryMock;

        [TestInitialize]
        public void SetUp() {
            _repositoryMock = new Mock<IDiffContentRepository>(MockBehavior.Strict);

            _service = new DiffService(_repositoryMock.Object);
        }

        [TestMethod]
        public void Should_create_new_instance() {
            var id = RandomString();
            var expected = new DiffModel(_service, id);

            var actual = _service.Create(id);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_save_into_repository() {
            _repositoryMock.Setup(mock => mock.Save(It.IsAny<DiffContent>()));

            var id = RandomString();
            var left = new byte[] { 1, 4, 5 };
            var right = new byte[] { 1, 5, 7 };
            new DiffModel(_service, id).SetLeft(left).SetRight(right).Save();

            _repositoryMock.Verify(mock => mock.Save(It.IsAny<DiffContent>()), Times.Once);
            _repositoryMock.Verify(mock => mock.Save(It.Is<DiffContent>(actual => id == actual.Id && left == actual.Left && right == actual.Right)));
        }

        [TestMethod]
        public void Should_return_default_when_not_exists() {
            var id = RandomString();
            _repositoryMock.Setup(mock => mock.Read(id)).Returns(default(DiffContent));

            var actual = _service.Load(id);
            Assert.IsNull(actual);

            _repositoryMock.Verify(mock => mock.Read(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Should_return_loaded_when_exists() {
            var id = RandomString();
            var left = new byte[] { 1, 4, 5 };
            var right = new byte[] { 1, 5, 7 };
            _repositoryMock.Setup(mock => mock.Read(id)).Returns(new DiffContent(id) {
                Left = left,
                Right = right
            });

            var expected = new DiffModel(_service, id).SetLeft(left).SetRight(right);

            var actual = _service.Load(id);
            Assert.AreEqual(expected, actual);

            _repositoryMock.Verify(mock => mock.Read(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Should_return_new_when_not_exists() {
            var id = RandomString();
            _repositoryMock.Setup(mock => mock.Read(id)).Returns(default(DiffContent));

            var expected = new DiffModel(_service, id);

            var actual = _service.LoadOrCreate(id);
            Assert.AreEqual(expected, actual);

            _repositoryMock.Verify(mock => mock.Read(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Should_return_loaded_when_exists_too() {
            var id = RandomString();
            var left = new byte[] { 1, 4, 5 };
            var right = new byte[] { 1, 5, 7 };
            _repositoryMock.Setup(mock => mock.Read(id)).Returns(new DiffContent(id) {
                Left = left,
                Right = right
            });

            var expected = new DiffModel(_service, id).SetLeft(left).SetRight(right);

            var actual = _service.LoadOrCreate(id);
            Assert.AreEqual(expected, actual);

            _repositoryMock.Verify(mock => mock.Read(It.IsAny<string>()), Times.Once);
        }

        private static string RandomString() => Guid.NewGuid().ToString();
    }
}