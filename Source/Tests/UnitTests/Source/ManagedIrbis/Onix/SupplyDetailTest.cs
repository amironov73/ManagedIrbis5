﻿// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SupplyDetailTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void SupplyDetail_Construction_1()
        {
            var detail = new SupplyDetail();
            Assert.IsNotNull (detail);
        }
    }
}
