// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.Text.Output;

using ManagedIrbis.Biblio;
using ManagedIrbis.Providers;
using ManagedIrbis.Reports;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio
{
    [TestClass]
    public sealed class BiblioProcessorTest
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
        [Description ("Конструктор по умолчанию")]
        public void BiblioProcessor_Construction_1()
        {
            var processor = new BiblioProcessor();
            Assert.IsNotNull (processor.Output);
            Assert.IsNotNull (processor.Report);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void BiblioProcessor_Construction_2()
        {
            var output = new ReportOutput();
            var processor = new BiblioProcessor (output);
            Assert.IsNotNull (processor.Output);
            Assert.AreSame (output, processor.Output);
        }

        [TestMethod]
        [Description ("Построение документа")]
        public void BiblioProcessor_Initialize_1()
        {
            var context = _GetContext();
            var processor = new BiblioProcessor();
            processor.Initialize (context);
            Assert.AreSame (processor, context.Processor);
        }

        [TestMethod]
        [Description ("Построение документа")]
        [ExpectedException (typeof (NotImplementedException))]
        public void BiblioProcessor_BuildDocument_1()
        {
            var context = _GetContext();
            var processor = new BiblioProcessor();
            processor.BuildDocument (context);
        }

        [TestMethod]
        [Description ("Получение форматтера")]
        [ExpectedException (typeof (NotImplementedException))]
        public void BiblioProcessor_AcquireFormatter_2()
        {
            var context = _GetContext();
            var processor = new BiblioProcessor();
            var formatter = processor.AcquireFormatter (context);
            processor.ReleaseFormatter (context, formatter);
        }

    }
}
