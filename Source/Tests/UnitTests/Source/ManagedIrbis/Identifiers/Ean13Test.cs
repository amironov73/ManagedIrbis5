﻿// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class Ean13Test
    {
        [TestMethod]
        [Description ("Вычисление контрольной цифры")]
        public void Ean13_ComputeCheckDigit_1()
        {
            Assert.AreEqual ('7', Ean13.ComputeCheckDigit ("4600051000057"));
        }

        [TestMethod]
        [Description ("Проверка контрольной цифры")]
        public void Ean13_CheckControlDigit_1()
        {
            Assert.IsTrue (Ean13.CheckControlDigit ("4600051000057"));
            Assert.IsFalse (Ean13.CheckControlDigit ("4600051000056"));
        }
    }
}
