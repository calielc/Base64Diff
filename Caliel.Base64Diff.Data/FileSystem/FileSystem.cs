using System.Diagnostics;

namespace Caliel.Base64Diff.Data.FileSystem {
    public sealed class SystemIO {
        public static readonly SystemIO Instance = new SystemIO(SystemIODirectory.Instance, SystemIOFile.Instance, SystemIOPath.Instance);

        internal SystemIO(ISystemIODirectory directory, ISystemIOFile file, ISystemIOPath path) {
            Debug.Assert(directory != null, nameof(directory));
            Debug.Assert(file != null, nameof(file));
            Debug.Assert(path != null, nameof(path));

            Directory = directory;
            File = file;
            Path = path;
        }

        public ISystemIODirectory Directory { get; }
        public ISystemIOFile File { get; }
        public ISystemIOPath Path { get; }
    }
}