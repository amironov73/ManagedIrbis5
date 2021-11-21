// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class MeasureTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void MeasureType_ListValues_1()
        {
            var values = MeasureType.ListValues();
            Assert.AreEqual (5, values.Length);
        }
    }
}
