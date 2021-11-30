// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Tables;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Tables
{
    [TestClass]
    public sealed class TbuFileTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void TbuFile_Construction_1()
        {
            var tbu = new TbuFile();
            Assert.IsNull (tbu.Encoding);
            Assert.IsNull (tbu.Header);
            Assert.IsNull (tbu.Table);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void TbuFile_Construction_2()
        {
            var tbu = new TbuFile()
            {
                Encoding = "111",
                Header = "222",
                Table = "333"
            };
            Assert.AreEqual ("111", tbu.Encoding);
            Assert.AreEqual ("222", tbu.Header);
            Assert.AreEqual ("333", tbu.Table);
        }

    }
}
