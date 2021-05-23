// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class Field203Test
    {
        [TestMethod]
        public void Field203_Construction_1()
        {
            var field = new Field203();
            Assert.IsNull(field.Access);
            Assert.IsNull(field.ContentDescription);
            Assert.IsNull(field.ContentType);
        }

        [TestMethod]
        public void Field203_Parse_1()
        {
            var field = new Field(203);
            var parsed = Field203.Parse(field);
            Assert.IsNotNull(parsed.Access);
            Assert.AreEqual(0, parsed.Access!.Length);
            Assert.IsNotNull(parsed.ContentDescription);
            Assert.AreEqual(0, parsed.ContentDescription!.Length);
            Assert.IsNotNull(parsed.ContentType);
            Assert.AreEqual(0, parsed.ContentType!.Length);
        }
    }
}
