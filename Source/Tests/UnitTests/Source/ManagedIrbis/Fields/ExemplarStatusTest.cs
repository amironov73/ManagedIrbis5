// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class ExemplarStatusTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void ExemplarStatus_ListValues_1()
        {
            var values = ExemplarStatus.ListValues();
            Assert.AreEqual (13, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void ExemplarStatus_ListValuesWithDescriptions_1()
        {
            var values = ExemplarStatus.ListValuesWithDescriptions();
            Assert.AreEqual (13, values.Count);
        }
    }
}
