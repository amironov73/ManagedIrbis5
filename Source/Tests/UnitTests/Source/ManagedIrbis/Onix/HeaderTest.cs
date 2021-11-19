// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class HeaderTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Header_Construction_1()
        {
            var header = new Header();
            Assert.IsNull (header.Addressee);
            Assert.IsNull (header.Sender);
            Assert.IsNull (header.MessageNote);
            Assert.IsNull (header.MessageNumber);
            Assert.IsNull (header.DefaultCurrencyCode);
            Assert.IsNull (header.DefaultPriceType);
            Assert.IsNull (header.SentDateTime);
            Assert.IsNull (header.DefaultLanguageOfText);
        }
    }
}
