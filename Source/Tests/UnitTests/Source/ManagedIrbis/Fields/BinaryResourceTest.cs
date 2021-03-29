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
        [TestMethod]
        public void BinaryResource_Constructor_1()
        {
            var resource = new BinaryResource();
            Assert.IsNull(resource.Kind);
            Assert.IsNull(resource.Resource);
            Assert.IsNull(resource.Title);
            Assert.IsNull(resource.View);
        }

        [TestMethod]
        public void BinaryResource_Encode_1()
        {
            var resource = new BinaryResource();
            byte[] bytes = {1, 2, 3, 4, 5};
            var actual = resource.Encode(bytes);
            const string expected = "%01%02%03%04%05";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BinaryResource_Decode_1()
        {
            var resource = new BinaryResource
            {
                Resource = "%01%02%03%04%05"
            };
            byte[] expected = {1, 2, 3, 4, 5};
            var actual = resource.Decode();
            CollectionAssert.AreEqual(expected, actual);
        }

        private Field _GetField()
        {
            var result = new Field { Tag = 953 }
                .Add('a', "jpg")
                .Add('b', "%01%02%03%04%05")
                .Add('t', "Cover");

            return result;
        }

        [TestMethod]
        public void BinaryResource_Parse_1()
        {
            var field = _GetField();
            var resource = BinaryResource.Parse(field);
            Assert.AreEqual("jpg", resource.Kind);
            Assert.AreEqual("%01%02%03%04%05", resource.Resource);
            Assert.AreEqual("Cover", resource.Title);
        }

        [TestMethod]
        public void BinaryResource_Parse_2()
        {
            var record = new Record();
            record.Fields.Add(_GetField());
            var resources= BinaryResource.Parse(record);
            Assert.AreEqual(1, resources.Length);
            Assert.AreEqual("jpg", resources[0].Kind);
            Assert.AreEqual("%01%02%03%04%05", resources[0].Resource);
            Assert.AreEqual("Cover", resources[0].Title);
        }

        [TestMethod]
        public void BinaryResource_Parse_3()
        {
            var record = new Record();
            record.Fields.Add(_GetField());
            var resources = BinaryResource.Parse(record);
            Assert.AreEqual(1, resources.Length);
            Assert.AreEqual("jpg", resources[0].Kind);
            Assert.AreEqual("%01%02%03%04%05", resources[0].Resource);
            Assert.AreEqual("Cover", resources[0].Title);
        }
    }
}
