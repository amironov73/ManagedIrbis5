// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Processing;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class ProtocolLineTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ProtocolLine_Construction_1()
        {
            var line = new ProtocolLine();
            Assert.AreEqual (false, line.Success);
            Assert.AreEqual (null, line.Database);
            Assert.AreEqual (0, line.Mfn);
            Assert.AreEqual (null, line.Update);
            Assert.AreEqual (null, line.Status);
            Assert.AreEqual (null, line.Error);
            Assert.AreEqual (null, line.UpdUf);
            Assert.AreEqual (null, line.Text);
        }

        [TestMethod]
        [Description("Декодирование текстового представления")]
        public void ProtocolLine_Decode_1()
        {
            var line = new ProtocolLine();
            line.Decode
                (
                    "DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#"
                );
            Assert.AreEqual (true, line.Success);
            Assert.AreEqual ("IBIS", line.Database);
            Assert.AreEqual (2, line.Mfn);
            Assert.AreEqual ("0", line.Update);
            Assert.AreEqual ("8", line.Status);
            Assert.AreEqual (null, line.Error);
            Assert.AreEqual ("0", line.UpdUf);
            Assert.AreEqual ("DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#", line.Text);
        }

        [TestMethod]
        [Description ("Декодирование текстового представления")]
        public void ProtocolLine_Decode_2()
        {
            var line = new ProtocolLine();
            line.Decode ("DBN=IBIS#MFN=4#GBL_ERROR=-605");
            Assert.AreEqual (false, line.Success);
            Assert.AreEqual ("IBIS", line.Database);
            Assert.AreEqual (4, line.Mfn);
            Assert.AreEqual (null, line.Update);
            Assert.AreEqual (null, line.Status);
            Assert.AreEqual ("-605", line.Error);
            Assert.AreEqual (null, line.UpdUf);
            Assert.AreEqual ("DBN=IBIS#MFN=4#GBL_ERROR=-605", line.Text);
        }

        [TestMethod]
        [Description ("Декодирование пустого ответа сервера")]
        public void ProtocolLine_Decode_3()
        {
            var mock = new Mock<ISetLastError>();
            var connection = mock.Object;

            var response = new Response (connection);
            var lines = ProtocolLine.Decode (response);
            Assert.IsNotNull (lines);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ProtocolLine_ToString_1()
        {
            var line = new ProtocolLine();
            line.Decode ("DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#");
            Assert.AreEqual
                (
                    "DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#",
                    line.ToString()
                );
        }
    }
}
