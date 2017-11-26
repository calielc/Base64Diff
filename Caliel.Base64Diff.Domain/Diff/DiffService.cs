using Caliel.Base64Diff.Data;

namespace Caliel.Base64Diff.Domain.Diff {
    internal class DiffService : IDiffService {
        private readonly IDiffContentRepository _diffRepository;

        public DiffService(IDiffContentRepository diffRepository) {
            _diffRepository = diffRepository;
        }

        public virtual DiffModel Create(string id)
            => new DiffModel(this, id);

        public virtual DiffModel Load(string id) {
            var data = _diffRepository.Read(id);
            if (data is null) {
                return default;
            }

            return Create(id).SetData(data);
        }

        public virtual DiffModel LoadOrCreate(string id)
            => Load(id) ?? Create(id);

        internal virtual void Save(string id, DiffContent content) {
            _diffRepository.Save(content);
        }
    }
}