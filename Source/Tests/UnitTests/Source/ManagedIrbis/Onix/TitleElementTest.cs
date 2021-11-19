// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class TitleElementTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void TitleElement_Construction_1()
        {
            var element = new TitleElement();
            Assert.IsNotNull (element);
        }
    }
}
