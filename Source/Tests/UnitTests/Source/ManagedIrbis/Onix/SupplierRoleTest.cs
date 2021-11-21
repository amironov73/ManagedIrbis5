// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SupplierRoleTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void SupplierRole_ListValues_1()
        {
            var values = SupplierRole.ListValues();
            Assert.AreEqual (4, values.Length);
        }
    }
}
