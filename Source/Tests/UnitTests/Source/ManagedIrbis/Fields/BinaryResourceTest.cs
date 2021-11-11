// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class BinaryResourceTest
    {
        private Field _GetField() =>  new Field (BinaryResource.Tag)
                .Add ('a', "jpg")
                .Add ('b', "%01%02%03%04%05")
                .Add ('t', "Cover");

        private BinaryResource _GetBinaryResource() => new ()
        {

            Kind = "jpg",
            Resource = "%01%02%03%04%05",
            Title = "Cover"
        };

        private void _Compare
            (
                BinaryResource first,
                BinaryResource second
            )
        {
            Assert.AreEqual (first.Kind, second.Kind);
            Assert.AreEqual (first.Resource, second.Resource);
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.View, second.View);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BinaryResource_Constructor_1()
        {
            var resource = new BinaryResource();
            Assert.IsNull (resource.Kind);
            Assert.IsNull (resource.Resource);
            Assert.IsNull (resource.Title);
            Assert.IsNull (resource.View);
            Assert.IsNull (resource.Field);
            Assert.IsNull (resource.UserData);
        }

        [TestMethod]
        [Description ("Кодирование ресурса в строковое представление")]
        public void BinaryResource_Encode_1()
        {
            var resource = new BinaryResource();
            byte[] bytes = { 1, 2, 3, 4, 5 };
            var actual = resource.Encode (bytes);
            const string expected = "%01%02%03%04%05";
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Декодирование ресурса")]
        public void BinaryResource_Decode_1()
        {
            var resource = new BinaryResource
            {
                Resource = "%01%02%03%04%05"
            };
            byte[] expected = { 1, 2, 3, 4, 5 };
            var actual = resource.Decode();
            CollectionAssert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void BinaryResource_ParseField_1()
        {
            var field = _GetField();
            var actual = BinaryResource.ParseField (field);
            var expected = _GetBinaryResource();
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void BinaryResource_ParseRecord_1()
        {
            var record = new Record();
            record.Fields.Add (_GetField());
            var actual = BinaryResource.ParseRecord (record);
            Assert.AreEqual (1, actual.Length);
        }

    }
}
