// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class TitleCharacterSetTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void TitleCharacterSet_ListValues_1()
        {
            var values = TitleCharacterSet.ListValues();
            Assert.AreEqual (16, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void TitleCharacterSet_ListValuesWithDescriptions_1()
        {
            var values = TitleCharacterSet.ListValuesWithDescriptions();
            Assert.AreEqual (16, values.Count);
        }
    }
}
