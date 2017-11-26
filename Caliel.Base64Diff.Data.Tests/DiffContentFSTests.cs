using Caliel.Base64Diff.Data.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliel.Base64Diff.Data.Tests {
    [TestClass]
    public sealed class DiffContentFSTests {
        private const string BasePath = "c:\\temp";
        private const string Id = "A\"<>|\0\t\n\r:*?\\/BC";
        private const string LeftPath = "c:\\temp\\ABC-left.bin";
        private const string RightPath = "c:\\temp\\ABC-right.bin";

        private SystemIODirectoryMock _directoryMock;
        private SystemIOFileMock _fileMock;
        private SystemIO _fileSystem;

        [TestInitialize]
        public void SetUp() {
            _directoryMock = new SystemIODirectoryMock();
            _fileMock = new SystemIOFileMock();

            _fileSystem = new SystemIO(_directoryMock.Object, _fileMock.Object, SystemIOPath.Instance);
        }

        [TestMethod]
        public void Should_not_create_directory() {
            _directoryMock.SetupExists.Returns(true);

            var unused = new DiffContentFS(BasePath, _fileSystem);

            _directoryMock.VerifyExists(BasePath);
            _directoryMock.VerifyCreateDirectory();
        }

        [TestMethod]
        public void Should_create_directory() {
            _directoryMock.SetupExists.Returns(false);

            var unused = new DiffContentFS(BasePath, _fileSystem);

            _directoryMock.VerifyExists(BasePath);
            _directoryMock.VerifyCreateDirectory(BasePath);
        }

        [TestMethod]
        public void Should_return_both_sides() {
            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            var left = new byte[] { 0, 1 };
            var right = new byte[] { 0, 1 };
            _fileMock.SetupReadAllBytes.Returns<string>(path => {
                switch (path) {
                    case LeftPath:
                        return left;
                    case RightPath:
                        return right;
                    default:
                        return null;
                }
            });

            var actual = diffContentFS.Read(Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(Id, actual.Id, nameof(actual.Id));
            Assert.AreEqual(left, actual.Left, nameof(actual.Left));
            Assert.AreEqual(right, actual.Right, nameof(actual.Right));

            _fileMock.VerifyExists(LeftPath, RightPath);
            _fileMock.VerifyReadAllBytes(LeftPath, RightPath);

        }

        [TestMethod]
        public void Should_return_left_sides() {
            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            _fileMock.SetupExists.Returns<string>(path => path == LeftPath);

            var left = new byte[] { 0, 1 };
            _fileMock.SetupReadAllBytes.Returns(left);

            var actual = diffContentFS.Read(Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(Id, actual.Id, nameof(actual.Id));
            Assert.AreEqual(left, actual.Left, nameof(actual.Left));
            Assert.IsNull(actual.Right, nameof(actual.Right));

            _fileMock.VerifyExists(LeftPath, RightPath);
            _fileMock.VerifyReadAllBytes(LeftPath);
        }

        [TestMethod]
        public void Should_return_right_sides() {
            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            _fileMock.SetupExists.Returns<string>(path => path == RightPath);

            var right = new byte[] { 0, 1 };
            _fileMock.SetupReadAllBytes.Returns(right);

            var actual = diffContentFS.Read(Id);

            Assert.IsNotNull(actual);
            Assert.AreEqual(Id, actual.Id, nameof(actual.Id));
            Assert.IsNull(actual.Left, nameof(actual.Left));
            Assert.AreEqual(right, actual.Right, nameof(actual.Right));

            _fileMock.VerifyExists(LeftPath, RightPath);
            _fileMock.VerifyReadAllBytes(RightPath);
        }

        [TestMethod]
        public void Should_return_none_sides() {
            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            _fileMock.SetupExists.Returns(false);

            var actual = diffContentFS.Read(Id);
            Assert.IsNull(actual);

            _fileMock.VerifyExists(LeftPath, RightPath);
            _fileMock.VerifyReadAllBytes();
        }

        [TestMethod]
        public void Should_save_both_sides() {
            var left = new byte[] { 0, 1 };
            var right = new byte[] { 0, 1 };

            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            diffContentFS.Save(new DiffContent(Id) {
                Left = left,
                Right = right
            });

            _fileMock.VerifyWriteAllBytes((LeftPath, left), (RightPath, right));
        }

        [TestMethod]
        public void Should_save_left_sides() {
            var left = new byte[] { 0, 1 };

            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            diffContentFS.Save(new DiffContent(Id) {
                Left = left,
            });

            _fileMock.VerifyWriteAllBytes((LeftPath, left));
            _fileMock.VerifyExists(RightPath);
            _fileMock.VerifyDelete(RightPath);
        }

        [TestMethod]
        public void Should_save_right_sides() {
            var right = new byte[] { 0, 1 };

            _fileMock.SetupExists.Returns(false);

            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            diffContentFS.Save(new DiffContent(Id) {
                Right = right,
            });

            _fileMock.VerifyWriteAllBytes((RightPath, right));
            _fileMock.VerifyExists(LeftPath);
            _fileMock.VerifyDelete();
        }

        [TestMethod]
        public void Should_save_none_sides() {
            _fileMock.SetupExists.Returns<string>(path => path == LeftPath);

            var diffContentFS = new DiffContentFS(BasePath, _fileSystem);

            diffContentFS.Save(new DiffContent(Id));

            _fileMock.VerifyWriteAllBytes();
            _fileMock.VerifyExists(LeftPath, RightPath);
            _fileMock.VerifyDelete(LeftPath);
        }
    }
}
