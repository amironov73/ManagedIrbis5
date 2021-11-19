// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class OnixMessageTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void OnixMessage_Construction_1()
        {
            var message = new OnixMessage();
            Assert.IsNotNull (message.Header);
            Assert.IsNotNull (message.Products);
        }
    }
}
