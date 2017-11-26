using System.Linq;
using Caliel.Base64Diff.Data.FileSystem;

namespace Caliel.Base64Diff.Data {
    public sealed class DiffContentFS : IDiffContentRepository {
        private readonly SystemIO _fileSystem;
        private readonly string _basePath;

        public DiffContentFS(string basePath) : this(basePath, SystemIO.Instance) { }

        internal DiffContentFS(string basePath, SystemIO fileSystem) {
            _fileSystem = fileSystem;
            if (_fileSystem.Directory.Exists(basePath) == false) {
                _fileSystem.Directory.CreateDirectory(basePath);
            }

            _basePath = basePath;
        }

        public DiffContent Read(string id) {
            var leftContent = Read("left");
            var rightContent = Read("right");
            if (leftContent is null && rightContent is null) {
                return default;
            }

            return new DiffContent(id) {
                Left = leftContent,
                Right = rightContent
            };

            byte[] Read(string side) {
                var path = CombinePath(id, side);
                return _fileSystem.File.Exists(path) ? _fileSystem.File.ReadAllBytes(path) : default;
            }
        }

        public void Save(DiffContent content) {
            Save("left", content.Left);
            Save("right", content.Right);

            void Save(string side, byte[] bytes) {
                var path = CombinePath(content.Id, side);
                if (bytes != null) {
                    _fileSystem.File.WriteAllBytes(path, bytes);
                }
                else if (_fileSystem.File.Exists(path)) {
                    _fileSystem.File.Delete(path);
                }
            }
        }

        private string CombinePath(string id, string side) {
            var invalidFileNameChars = _fileSystem.Path.GetInvalidFileNameChars();
            id = new string(id.Except(invalidFileNameChars).ToArray());

            return _fileSystem.Path.Combine(_basePath, $"{id}-{side}.bin");
        }
    }
}