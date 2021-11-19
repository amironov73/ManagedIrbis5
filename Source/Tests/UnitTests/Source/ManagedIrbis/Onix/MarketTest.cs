// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class MarketTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Market_Construction_1()
        {
            var market = new Market();
            Assert.IsNull (market.Territory);
        }
    }
}
