// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SupplyDateTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void SupplyDate_Construction_1()
        {
            var date = new SupplyDate();
            Assert.IsNotNull (date);
        }
    }
}
