// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text.Output;

using ManagedIrbis.Biblio;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio
{
    [TestClass]
    public sealed class BiblioContextTest
    {
        private BiblioContext _GetContext()
        {
            return new BiblioContext
                (
                    new BiblioDocument(),
                    new NullProvider(),
                    new NullOutput()
                );
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void BiblioContext_Construction_1()
        {
            var document = new BiblioDocument();
            var provider = new NullProvider();
            var output = new NullOutput();
            var context = new BiblioContext(document, provider, output);
            Assert.AreSame (document, context.Document);
            Assert.AreSame (provider, context.Provider);
            Assert.AreSame (output, context.Log);
            Assert.IsNotNull (context.ReportContext);
            Assert.IsNotNull (context.Records);
            Assert.IsNotNull (context.BadRecords);
            Assert.AreEqual (0, context.ItemCount);
        }

        [TestMethod]
        [Description ("Поиск записи")]
        public void BiblioContext_FindRecord_1()
        {
            var context = _GetContext();
            Assert.IsNull (context.FindRecord (123));
        }

        [TestMethod]
        [Description ("Верификация")]
        public void BiblioContext_Verify_1()
        {
            var context = _GetContext();
            Assert.IsTrue (context.Verify (false));
        }

    }
}
