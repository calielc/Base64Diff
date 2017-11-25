using System;
using System.Text;
using Caliel.Base64Diff.Api.v1.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliel.Base64Diff.Api.Tests.Unit {
    [TestClass]
    public sealed class DiffInputModelTests {
        [TestMethod]
        [DataRow("Caliel")]
        [DataRow("Costa")]
        public void Should_return_is_valid(string text) {
            var bytes = Encoding.UTF8.GetBytes(text);

            var model = new DiffInputModel {
                Data = Convert.ToBase64String(bytes)
            };

            var actual = model.IsValid();
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("82349lfks")]
        public void Should_return_is_invalid(string base64String) {
            var model = new DiffInputModel {
                Data = base64String
            };

            var actual = model.IsValid();
            Assert.IsFalse(actual);
        }

        [TestMethod]
        [DataRow("Caliel")]
        [DataRow("Costa")]
        public void Should_return_bytes(string text) {
            var expected = Encoding.UTF8.GetBytes(text);

            var model = new DiffInputModel {
                Data = Convert.ToBase64String(expected)
            };

            var actual = model.GetBytes();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("82349lfks")]
        public void Should_throw_exception(string base64String) {
            var model = new DiffInputModel {
                Data = base64String
            };

            model.GetBytes();
        }
    }
}
