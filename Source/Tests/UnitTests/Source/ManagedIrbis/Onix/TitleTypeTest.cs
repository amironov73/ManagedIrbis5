// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class TitleTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void TitleType_ListValues_1()
        {
            var values = TitleType.ListValues();
            Assert.AreEqual (11, values.Length);
        }
    }
}
