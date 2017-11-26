using System;
using Caliel.Base64Diff.Data.FileSystem;
using Moq;
using Moq.Language.Flow;

namespace Caliel.Base64Diff.Data.Tests {
    internal sealed class SystemIODirectoryMock {
        public SystemIODirectoryMock() {
            Moq = new Mock<ISystemIODirectory>();

            SetupExists = Moq.Setup(mock => mock.Exists(It.IsAny<string>()));
            SetupExists.Returns(true);

            SetupCreateDirectory = Moq.Setup(mock => mock.CreateDirectory(It.IsAny<string>()));
        }

        public Mock<ISystemIODirectory> Moq { get; }
        public ISystemIODirectory Object => Moq.Object;

        public ISetup<ISystemIODirectory> SetupCreateDirectory { get; }
        public ISetup<ISystemIODirectory, bool> SetupExists { get; }

        public void VerifyExists(params string[] paths) {
            Moq.Verify(mock => mock.Exists(It.IsAny<string>()), Times.Exactly(paths.Length));

            foreach (var path in paths) {
                Moq.Verify(mock => mock.Exists(path));
            }

            return;
        }

        public void VerifyCreateDirectory(params string[] paths) {
            Moq.Verify(mock => mock.CreateDirectory(It.IsAny<string>()), Times.Exactly(paths.Length));

            foreach (var path in paths) {
                Moq.Verify(mock => mock.CreateDirectory(path));
            }
            return;
        }
    }
}