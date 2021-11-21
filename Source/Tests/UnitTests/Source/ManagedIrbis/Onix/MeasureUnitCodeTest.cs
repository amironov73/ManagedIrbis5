// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class MeasureUnitCodeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void MeasureUnitCode_ListValues_1()
        {
            var values = MeasureUnitCode.ListValues();
            Assert.AreEqual (4, values.Length);
        }
    }
}
