using System.Collections.Generic;

namespace Caliel.Base64Diff.Domain.Diff {
    internal class DiffService : IDiffService {
        private static readonly Dictionary<string, DiffData> FakeDB = new Dictionary<string, DiffData>();

        public virtual DiffModel Create(string id)
            => new DiffModel(this, id);

        public virtual DiffModel Load(string id)
            => FakeDB.TryGetValue(id, out var result)
            ? Create(id).SetData(result)
            : default;

        public virtual DiffModel LoadOrCreate(string id)
            => Load(id) ?? Create(id);

        internal virtual void Save(string id, DiffData data) {
            FakeDB[id] = data;
        }
    }
}