// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class Upc12Test
    {
        [TestMethod]
        [Description ("Вычисление контрольной цифры")]
        public void Upc12_ComputeCheckDigit_1()
        {
            Assert.AreEqual ('4', Upc12.ComputeCheckDigit ("041689300494".ToCharArray()));
        }

        [TestMethod]
        [Description ("Проверка контрольной цифры")]
        public void Upc12_CheckControlDigit_1()
        {
            Assert.IsTrue (Upc12.CheckControlDigit ("041689300494".ToCharArray()));
            Assert.IsFalse (Upc12.CheckControlDigit ("041689300493".ToCharArray()));
        }
    }
}
