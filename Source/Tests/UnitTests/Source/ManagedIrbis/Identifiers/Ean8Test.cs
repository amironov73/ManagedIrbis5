// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class Ean8Test
    {
        [TestMethod]
        [Description ("Вычисление контрольной цифры")]
        public void Ean8_ComputeCheckDigit_1()
        {
            Assert.AreEqual ('3', Ean8.ComputeCheckDigit ("46009333"));
        }

        [TestMethod]
        [Description ("Проверка контрольной цифры")]
        public void Ean8_CheckControlDigit_1()
        {
            Assert.IsTrue (Ean8.CheckControlDigit ("46009333"));
            Assert.IsFalse (Ean8.CheckControlDigit ("46009332"));
        }
    }
}
