// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class WebsiteTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Website_Construction_1()
        {
            var website = new Website();
            Assert.IsNotNull (website);
        }
    }
}
