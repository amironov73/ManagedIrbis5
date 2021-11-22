// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class WorkPhaseTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void WorkPhase_ListValues_1()
        {
            var values = WorkPhase.ListValues();
            Assert.AreEqual (15, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void WorkPhase_ListValuesWithDescriptions_1()
        {
            var values = WorkPhase.ListValuesWithDescriptions();
            Assert.AreEqual (15, values.Count);
        }
    }
}
