// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class LanguageCodeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void LanguageCode_ListValues_1()
        {
            var values = LanguageCode.ListValues();
            Assert.AreEqual (22, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void LanguageCode_ListValuesWithDescriptions_1()
        {
            var values = LanguageCode.ListValuesWithDescriptions();
            Assert.AreEqual (22, values.Count);
        }
    }
}
