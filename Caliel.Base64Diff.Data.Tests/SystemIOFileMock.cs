using Caliel.Base64Diff.Data.FileSystem;
using Moq;
using Moq.Language.Flow;

namespace Caliel.Base64Diff.Data.Tests {
    internal sealed class SystemIOFileMock {
        public SystemIOFileMock() {
            Moq = new Mock<ISystemIOFile>();

            SetupExists = Moq.Setup(mock => mock.Exists(It.IsAny<string>()));
            SetupExists.Returns(true);

            SetupDelete = Moq.Setup(mock => mock.Delete(It.IsAny<string>()));

            SetupReadAllBytes = Moq.Setup(mock => mock.ReadAllBytes(It.IsAny<string>()));
            SetupReadAllBytes.Returns(new byte[0]);

            SetupWriteAllBytes = Moq.Setup(mock => mock.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()));
        }

        public Mock<ISystemIOFile> Moq { get; }
        public ISystemIOFile Object => Moq.Object;

        public ISetup<ISystemIOFile, bool> SetupExists { get; }
        public ISetup<ISystemIOFile> SetupDelete { get; }
        public ISetup<ISystemIOFile, byte[]> SetupReadAllBytes { get; }
        public ISetup<ISystemIOFile> SetupWriteAllBytes { get; }

        public void VerifyExists(params string[] paths) {
            Moq.Verify(mock => mock.Exists(It.IsAny<string>()), Times.Exactly(paths.Length));

            foreach (var path in paths) {
                Moq.Verify(mock => mock.Exists(path));
            }
        }

        public void VerifyReadAllBytes(params string[] paths) {
            Moq.Verify(mock => mock.ReadAllBytes(It.IsAny<string>()), Times.Exactly(paths.Length));
            foreach (var path in paths) {
                Moq.Verify(mock => mock.ReadAllBytes(path));
            }
        }

        public void VerifyWriteAllBytes(params (string path, byte[] bytes)[] tuples) {
            Moq.Verify(mock => mock.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Exactly(tuples.Length));
            foreach (var tuple in tuples) {
                Moq.Verify(mock => mock.WriteAllBytes(tuple.path, tuple.bytes));
            }
        }

        public void VerifyDelete(params string[] paths) {
            Moq.Verify(mock => mock.Delete(It.IsAny<string>()), Times.Exactly(paths.Length));

            foreach (var path in paths) {
                Moq.Verify(mock => mock.Delete(path));
            }
        }
    }
}