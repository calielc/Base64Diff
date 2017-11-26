using System;
using System.Net;
using Caliel.Base64Diff.Tests.ApiSDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliel.Base64Diff.Tests {
    [TestClass]
    public sealed class DiffControllerTests {
        private DiffApiResource _resource;

        [TestInitialize]
        public void SetUp() {
            ResetDatabase();

            _resource = new DiffApiResource($"{ApiResource.Host}/v1/diff/");
        }

        private static void ResetDatabase() {
            const string directory = "C:\\Temp\\diff";
            if (System.IO.Directory.Exists(directory)) {
                System.IO.Directory.Delete(directory, true);
            }
            System.IO.Directory.CreateDirectory(directory);
        }

        [TestMethod]
        public void Should_return_ok_when_sent_content_to_left() {
            var id = RandomString();
            var response = _resource.PostLeft(id, "Caliel Lima da Costa");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("82349lfks")]
        public void Should_return_badRequest_when_sent_invalid_content_to_left(string base64String) {
            var id = RandomString();
            var response = _resource.PostLeft(id, new DiffApiResource.PostModel(base64String));

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_ok_when_sent_content_to_right() {
            var id = RandomString();
            var response = _resource.PostRight(id, "Caliel Lima da Costa");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("82349lfks")]
        public void Should_return_badRequest_when_sent_invalid_content_to_right(string base64String) {
            var id = RandomString();
            var response = _resource.PostRight(id, new DiffApiResource.PostModel(base64String));

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_notFound_when_not_set_left_and_right() {
            var id = RandomString();
            var response = _resource.Get<string>(id);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, nameof(response.StatusCode));
            Assert.AreEqual(id, response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_notFound_when_not_set_left() {
            var id = RandomString();
            _resource.PostRight(id, "Caliel");

            var response = _resource.Get<string>(id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, nameof(response.StatusCode));
            Assert.IsNull(response.Content, nameof(response.Content));
        }

        [TestMethod]
        public void Should_return_notFound_when_not_set_right() {
            var id = RandomString();
            _resource.PostLeft(id, "Caliel");

            var response = _resource.Get<string>(id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, nameof(response.StatusCode));
            Assert.IsNull(response.Content, nameof(response.Content));
        }

        [TestMethod]
        [DataRow("Caliel", "Caliel")]
        [DataRow("costa", "costa")]
        public void Should_return_ok_and_AreEquals(string left, string right) {
            var id = RandomString();
            _resource.PostLeft(id, left);
            _resource.PostRight(id, right);

            var response = _resource.Get<DiffApiResource.GetModel>(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));

            var content = response.Content;
            Assert.AreEqual("AreEquals", content.Similarity, nameof(content.Similarity));
            Assert.IsNull(content.Diffs, nameof(content.Diffs));
        }

        [TestMethod]
        [DataRow("Caliel", "Calie")]
        [DataRow("Calie", "Caliel")]
        public void Should_return_ok_and_NotEqualSize(string left, string right) {
            var id = RandomString();
            _resource.PostLeft(id, left);
            _resource.PostRight(id, right);

            var response = _resource.Get<DiffApiResource.GetModel>(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));

            var responseContent = response.Content;
            Assert.AreEqual("NotEqualSize", responseContent.Similarity, nameof(responseContent.Similarity));
            Assert.IsNull(responseContent.Diffs, nameof(responseContent.Diffs));
        }

        [TestMethod]
        public void Should_return_ok_and_SameSize() {
            var id = RandomString();
            _resource.PostLeft(id, "Caliel Lima da Costa");
            _resource.PostRight(id, "Caliel lIMA dA CoSTa");

            var expectedDiffs = new[] {
                new DiffApiResource.OffsetLengthModel {Offset = 7, Length = 4},
                new DiffApiResource.OffsetLengthModel {Offset = 13, Length = 1},
                new DiffApiResource.OffsetLengthModel {Offset = 17, Length = 2},
            };

            var response = _resource.Get<DiffApiResource.GetModel>(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, nameof(response.StatusCode));

            var responseContent = response.Content;
            Assert.AreEqual("SameSize", responseContent.Similarity, nameof(responseContent.Similarity));
            CollectionAssert.AreEqual(expectedDiffs, responseContent.Diffs, nameof(responseContent.Diffs));
        }

        private static string RandomString() => Guid.NewGuid().ToString();
    }
}

