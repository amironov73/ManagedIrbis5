// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class CharacterSetCodeTest
    {
        [Description ("Получение массива значений констант")]
        public void CharacterSetCode_ListValues_1()
        {
            var values = CharacterSetCode.ListValues();
            Assert.AreEqual (13, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void CharacterSetCode_ListValuesWithDescriptions_1()
        {
            var values = CharacterSetCode.ListValuesWithDescriptions();
            Assert.AreEqual (13, values.Count);
        }
    }
}
