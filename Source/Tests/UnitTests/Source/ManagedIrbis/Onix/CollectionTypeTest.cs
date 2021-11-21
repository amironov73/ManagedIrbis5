// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class CollectionTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void CollectionType_ListValues_1()
        {
            var values = CollectionType.ListValues();
            Assert.AreEqual (3, values.Length);
        }
    }
}
