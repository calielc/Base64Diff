using Caliel.Base64Diff.Domain.Diff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliel.Base64Diff.Domain.Tests.Diff {
    [TestClass]
    public sealed class DiffServiceTests {
        private DiffService _service;

        [TestInitialize]
        public void SetUp() {
            _service = new DiffService();
        }

        [TestMethod]
        public void Should_return_new_instance() {
            const string id = "slfdjkaçlsd";
            var expected = new DiffModel(_service, id);

            var actual = _service.Create(id);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_return_default_when_not_exists() {
            const string id = "jfakslfjsfdsa";
            var actual = _service.Load(id);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Should_return_saved_instance() {
            const string id = "jfakslfjs";
            var left = new byte[] { 1, 4, 5 };
            var right = new byte[] { 1, 5, 7 };
            var expected = new DiffModel(_service, id).SetLeft(left).SetRight(right);

            var created = _service.Create(id).SetLeft(left).SetRight(right);
            created.Save();

            var actual = _service.Load(id);

            Assert.AreNotSame(created, actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_return_new_instance_when_not_exists() {
            const string id = "980-32klj";
            var expected = new DiffModel(_service, id);

            var actual = _service.LoadOrCreate(id);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_return_save_instance_when_exists() {
            const string id = "hfgdhgfhfgdhgfd";
            var left = new byte[] { 1, 4, 5 };
            var right = new byte[] { 1, 5, 7 };
            var expected = new DiffModel(_service, id).SetLeft(left).SetRight(right);

            var created = _service.Create(id).SetLeft(left).SetRight(right);
            created.Save();

            var actual = _service.LoadOrCreate(id);

            Assert.AreNotSame(created, actual);
            Assert.AreEqual(expected, actual);
        }
    }
}