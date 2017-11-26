namespace Caliel.Base64Diff.Data {
    public class DiffContent {
        public DiffContent(string id) {
            Id = id;
        }

        public string Id { get; }
        public byte[] Left { get; set; }
        public byte[] Right { get; set; }
    }
}