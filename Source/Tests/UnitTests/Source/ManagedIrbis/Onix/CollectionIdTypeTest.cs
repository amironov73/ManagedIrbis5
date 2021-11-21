// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class CollectionIdTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void CollectionIdType_ListValues_1()
        {
            var values = CollectionIdType.ListValues();
            Assert.AreEqual (3, values.Length);
        }
    }
}
