﻿using System.Collections.Generic;
using Caliel.Base64Diff.Data;
using Caliel.Base64Diff.Domain.Diff;
using Caliel.Base64Diff.Domain.Similarity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Caliel.Base64Diff.Domain.Tests.Diff {
    [TestClass]
    public sealed class DiffModelTests {
        private Mock<DiffService> _serviceMock;

        [TestInitialize]
        public void SetUp() {
            _serviceMock = new Mock<DiffService>(MockBehavior.Strict, null);
        }

        [TestMethod]
        public void Should_build_instance() {
            const string id = "dasjdkasl";
            var model = new DiffModel(_serviceMock.Object, id);

            Assert.AreEqual(id, model.Id, nameof(model.Id));
            Assert.AreSame(_serviceMock.Object, model.Owner, nameof(model.Owner));
            Assert.IsNull(model.Left, nameof(model.Left));
            Assert.IsNull(model.Right, nameof(model.Right));
        }

        [TestMethod]
        public void Should_set_left_array() {
            const string id = "dasjdkasl";
            var model = new DiffModel(_serviceMock.Object, id);

            var expected = new byte[] { 1, 2, 3 };
            var actual = model.SetLeft(expected);

            Assert.AreSame(actual, model);
            Assert.AreSame(expected, model.Left, nameof(model.Left));
            Assert.IsNull(model.Right, nameof(model.Right));
        }

        [TestMethod]
        public void Should_set_right_array() {
            const string id = "dsgfasdf";
            var model = new DiffModel(_serviceMock.Object, id);

            var expected = new byte[] { 1, 2, 3 };
            var actual = model.SetRight(expected);

            Assert.AreSame(actual, model);
            Assert.AreSame(expected, model.Right, nameof(model.Right));
            Assert.IsNull(model.Left, nameof(model.Left));
        }

        [TestMethod]
        [DataRow(null, new byte[] { 1, 2 })]
        [DataRow(new byte[] { 1, 2 }, null)]
        [DataRow(null, null)]
        public void Should_return_null_when_no_left_or_side(byte[] left, byte[] right) {
            const string id = "dsgfdsfsdfasdf";
            var model = new DiffModel(_serviceMock.Object, id).SetLeft(left).SetRight(right);

            var actual = model.Similarity;

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Should_return_null_when_no_left_or_side() {
            const string id = "dsgfdsfsdfasdf";
            var left = new byte[] { 4, 5, 6 };
            var right = new byte[] { 3, 8, 7, 9 };
            var model = new DiffModel(_serviceMock.Object, id).SetLeft(left).SetRight(right);

            var actual = model.Similarity as BytesSimilarity;
            Assert.IsNotNull(actual);
            Assert.AreEqual(left, actual.Left, nameof(actual.Left));
            Assert.AreEqual(right, actual.Right, nameof(actual.Right));
        }

        [TestMethod]
        public void Should_update_read_data() {
            const string id = "dsgfdsfsdfasdf";
            var model = new DiffModel(_serviceMock.Object, id);

            var left = new byte[] { 4, 5, 6 };
            var right = new byte[] { 3, 8, 7, 9 };

            var actual = model.SetData(new DiffContent(id) {
                Left = left,
                Right = right
            });

            Assert.AreSame(actual, model);
            Assert.AreSame(left, model.Left, nameof(model.Left));
            Assert.AreSame(right, model.Right, nameof(model.Right));
        }

        [TestMethod]
        public void Should_save_data() {
            _serviceMock.Setup(mock => mock.Save(It.IsAny<string>(), It.IsAny<DiffContent>()));

            const string id = "dsgfdsfsdfasdf";
            var left = new byte[] { 4, 5, 6 };
            var right = new byte[] { 3, 8, 7, 9 };
            var model = new DiffModel(_serviceMock.Object, id).SetLeft(left).SetRight(right);

            model.Save();

            _serviceMock.Verify(mock => mock.Save(It.IsAny<string>(), It.IsAny<DiffContent>()), Times.Once());
            _serviceMock.Verify(mock => mock.Save(id, It.Is<DiffContent>(actual => actual.Left == left && actual.Right == right)));
        }

        private static IEnumerable<object[]> CasesEquals() {
            var service = new Mock<DiffService>(MockBehavior.Strict, null).Object;
            const string id = "dsgfdsfsdfasdf";
            var left = new byte[] { 1, 2 };
            var right = new byte[] { 5, 6 };
            var model = new DiffModel(service, id).SetLeft(left).SetRight(right);

            var servico2 = new Mock<DiffService>(MockBehavior.Strict, null).Object;


            yield return new object[] { true, model, model };
            yield return new object[] { true, model, new DiffModel(service, id).SetLeft(left).SetRight(right) };

            yield return new object[] { false, model, null };
            yield return new object[] { false, model, new DiffModel(servico2, id).SetLeft(left).SetRight(right) };
            yield return new object[] { false, model, new DiffModel(service, "fasdfds").SetLeft(left).SetRight(right) };
            yield return new object[] { false, model, new DiffModel(service, id).SetLeft(new byte[] { 5 }).SetRight(right) };
            yield return new object[] { false, model, new DiffModel(service, id).SetLeft(left).SetRight(new byte[] { 5 }) };
        }

        [TestMethod]
        [DynamicData(nameof(CasesEquals), DynamicDataSourceType.Method)]
        public void Should_compare_with_class(bool expected, DiffModel model1, DiffModel model2) {
            var actual = model1.Equals(model2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(CasesEquals), DynamicDataSourceType.Method)]
        public void Should_compare_with_object(bool expected, DiffModel model1, DiffModel model2) {
            var actual = model1.Equals((object)model2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(CasesEquals), DynamicDataSourceType.Method)]
        public void Should_compare_with_equal_operator(bool expected, DiffModel model1, DiffModel model2) {
            var actual = model1 == model2;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(CasesEquals), DynamicDataSourceType.Method)]
        public void Should_compare_with_unequal_operator(bool expected, DiffModel model1, DiffModel model2) {
            var actual = model1 != model2;

            Assert.AreNotEqual(expected, actual);
        }
    }
}
