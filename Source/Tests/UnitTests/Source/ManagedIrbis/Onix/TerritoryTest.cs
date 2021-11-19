// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class TerritoryTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Territory_Construction_1()
        {
            var territory = new Territory();
            Assert.IsNotNull (territory);
        }
    }
}
