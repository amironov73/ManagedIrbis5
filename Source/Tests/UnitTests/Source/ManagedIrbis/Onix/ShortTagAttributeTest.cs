// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class ShortTagAttributeTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void ShortTagAttribute_Construction_1()
        {
            const string tag = "abc";
            var attribute = new ShortTagAttribute (tag);
            Assert.AreEqual (tag, attribute.Tag);
        }
    }
}
