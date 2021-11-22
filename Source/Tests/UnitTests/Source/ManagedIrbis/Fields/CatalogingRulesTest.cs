// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class CatalogingRulesTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void CatalogingRules_ListValues_1()
        {
            var values = CatalogingRules.ListValues();
            Assert.AreEqual (4, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void CatalogingRules_ListValuesWithDescriptions_1()
        {
            var values = CatalogingRules.ListValuesWithDescriptions();
            Assert.AreEqual (4, values.Count);
        }
    }
}
