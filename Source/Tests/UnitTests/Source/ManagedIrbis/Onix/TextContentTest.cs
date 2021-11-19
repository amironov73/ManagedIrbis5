// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class TextContentTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void TextContent_Construction_1()
        {
            var content = new TextContent();
            Assert.IsNotNull (content);
        }
    }
}
