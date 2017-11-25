using System.Net;
using Caliel.Base64Diff.Api.Tests.Integration.ApiSDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliel.Base64Diff.Api.Tests.Integration {
    [TestClass]
    public sealed class DiffControllerTests {
        private DiffApiResource _resource;

        [TestInitialize]
        public void SetUp() {
            _resource = new DiffApiResource("http://localhost:63635/v1/diff/");
        }

        [TestMethod]
        public void Should_return_ok_when_sent_content_to_left() {
            const string id = "def";
            var response = _resource.PostLeft(id, "Caliel Lima da Costa");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_badRequest_when_sent_no_content_to_left() {
            const string id = "def";
            var response = _resource.PostLeft(id, string.Empty);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_badRequest_when_sent_invalid_content_to_left() {
            const string id = "def";
            var response = _resource.PostLeft(id, new DiffApiResource.PostModel("82349lfks"));

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_ok_when_sent_content_to_right() {
            const string id = "abc";
            var response = _resource.PostRight(id, "Caliel Lima da Costa");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_badRequest_when_sent_no_content_to_right() {
            const string id = "def";
            var response = _resource.PostRight(id, string.Empty);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_badRequest_when_sent_invalid_content_to_right() {
            const string id = "def";
            var response = _resource.PostRight(id, new DiffApiResource.PostModel("Ajdksajdsak"));

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_notFound_when_not_set_left_and_right() {
            const string id = "def";
            var response = _resource.Get<string>(id);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_notFound_when_not_set_left() {
            const string id = "dsfsadf";
            _resource.PostRight(id, "Caliel");

            var response = _resource.Get<string>(id);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_notFound_when_not_set_right() {
            const string id = "24142";
            _resource.PostLeft(id, "Caliel");

            var response = _resource.Get<string>(id);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_ok_and_AreEquals() {
            const string id = "fsadfsda";
            _resource.PostLeft(id, "Caliel");
            _resource.PostRight(id, "Caliel");

            var response = _resource.Get<DiffApiResource.GetModel>(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));

            Assert.AreEqual<string>("AreEquals", response.Content.Situation, nameof(DiffApiResource.GetModel.Situation));
            Assert.IsNull(response.Content.Diffs, nameof(DiffApiResource.GetModel.Diffs));
        }

        [TestMethod]
        public void Should_return_ok_and_NotEqualSize() {
            const string id = "4231423";
            _resource.PostLeft(id, "Caliel");
            _resource.PostRight(id, "Calie");

            var response = _resource.Get<DiffApiResource.GetModel>(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));

            Assert.AreEqual<string>("NotEqualSize", response.Content.Situation, nameof(DiffApiResource.GetModel.Situation));
            Assert.IsNull(response.Content.Diffs, nameof(DiffApiResource.GetModel.Diffs));
        }

        [TestMethod]
        public void Should_return_ok_and_SameSize() {
            const string id = "dgfsgfds";
            _resource.PostLeft(id, "Caliel Lima da Costa");
            _resource.PostRight(id, "Caliel lIMA dA CoSTa");

            var expectedDiffs = new[] {
                new DiffApiResource.OffsetLengthModel {Offset = 7, Length = 4},
                new DiffApiResource.OffsetLengthModel {Offset = 13, Length = 1},
                new DiffApiResource.OffsetLengthModel {Offset = 17, Length = 2},
            };

            var response = _resource.Get<DiffApiResource.GetModel>(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual<string>("SameSize", response.Content.Situation, nameof(DiffApiResource.GetModel.Situation));
            CollectionAssert.AreEqual(expectedDiffs, response.Content.Diffs, nameof(DiffApiResource.GetModel.Diffs));
        }
    }

}

