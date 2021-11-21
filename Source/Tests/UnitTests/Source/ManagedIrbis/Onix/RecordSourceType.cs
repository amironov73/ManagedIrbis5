// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class RecordSourceTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void RecordSourceType_ListValues_1()
        {
            var values = RecordSourceType.ListValues();
            Assert.AreEqual (6, values.Length);
        }
    }
}
