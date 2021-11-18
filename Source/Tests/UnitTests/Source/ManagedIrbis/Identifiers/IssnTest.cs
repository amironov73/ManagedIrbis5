// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class IssnTest
    {
        [TestMethod]
        [Description ("Вычисление контрольной цифры")]
        public void Issn_ComputeCheckDigit_1()
        {
            Assert.AreEqual ('X', Issn.ComputeCheckDigit ("0033765X"));
        }

        [TestMethod]
        [Description ("Проверка контрольной цифры: простой случай")]
        public void Issn_CheckControlDigit_1()
        {
            Assert.IsTrue (Issn.CheckControlDigit ("0033765X"));
            Assert.IsFalse (Issn.CheckControlDigit ("00337651"));
        }

        [TestMethod]
        [Description ("Проверка контрольной цифры: неверная длина")]
        public void Issn_CheckControlDigit_2()
        {
            Assert.IsFalse (Issn.CheckControlDigit ("003376X"));
        }
    }
}
