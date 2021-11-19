// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class AddresseeTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Addressee_Construction_1()
        {
            var addressee = new Addressee();
            Assert.IsNull (addressee.Id);
            Assert.IsNull (addressee.Name);
            Assert.IsNull (addressee.ContactName);
            Assert.IsNull (addressee.EmailAddress);
        }
    }
}
