// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.LogClientDb;

#nullable enable

namespace UnitTests.ManagedIrbis.LogClientDb
{
    [TestClass]
    public sealed class SearchPrefixTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void SearchPrefix_ListValues_1()
        {
            var values = SearchPrefix.ListValues();
            Assert.AreEqual (5, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void SearchPrefix_ListValuesWithDescriptions_1()
        {
            var values = SearchPrefix.ListValuesWithDescriptions();
            Assert.AreEqual (5, values.Count);
        }
    }
}
