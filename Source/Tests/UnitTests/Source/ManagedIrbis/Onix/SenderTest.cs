// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SenderTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Sender_Construction_1()
        {
            var sender = new Sender();
            Assert.IsNull (sender.Id);
            Assert.IsNull (sender.ContactName);
            Assert.IsNull (sender.EmailAddress);
            Assert.IsNull (sender.SenderName);
        }
    }
}
