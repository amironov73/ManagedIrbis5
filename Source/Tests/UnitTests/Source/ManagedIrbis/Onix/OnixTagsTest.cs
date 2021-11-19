// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class OnixTagsTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void OnixTags_Construction_1()
        {
            var tags = new OnixTags (typeof (int), "System.Int32", "int");
            Assert.AreEqual (typeof (int), tags.Type);
            Assert.AreEqual ("System.Int32", tags.Reference);
            Assert.AreEqual ("int", tags.Short);
        }
    }
}
