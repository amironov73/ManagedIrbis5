// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Readers;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ChairInfoTest
    {
        [TestMethod]
        public void ChairInfo_Construction_1()
        {
            var chair = new ChairInfo();
            Assert.IsNull (chair.Code);
            Assert.IsNull (chair.Title);
        }

        [TestMethod]
        public void ChairInfo_Construction_2()
        {
            const string code = "АБ";
            var chair = new ChairInfo (code);
            Assert.AreEqual (code, chair.Code);
            Assert.IsNull (chair.Title);
        }

        [TestMethod]
        public void ChairInfo_Construction_3()
        {
            const string code = "АБ", title = "Абонемент";
            var chair = new ChairInfo (code, title);
            Assert.AreEqual (code, chair.Code);
            Assert.AreEqual (title, chair.Title);
        }

        private string _GetMenuText()
        {
            return "ЧЗ\r\nЧитальный зал\r\n" +
                   "АБ\r\nАбонемент\r\n" +
                   "ИБО\r\nБиблиографический отдел\r\n" +
                   "НМО\r\nМетодический отдел\r\n" +
                   "КХ\r\nКнигохранилище\r\n" +
                   "*****\r\n";
        }

        [TestMethod]
        public void ChairInfo_Parse_1()
        {
            var chairs = ChairInfo.Parse (_GetMenuText(), true);
            Assert.AreEqual (6, chairs.Length);
            Assert.AreEqual ("*", chairs[0].Code);
            Assert.AreEqual ("Все подразделения", chairs[0].Title);
            Assert.AreEqual ("ЧЗ", chairs[chairs.Length - 1].Code);
            Assert.AreEqual ("Читальный зал", chairs[chairs.Length - 1].Title);
        }

        [TestMethod]
        public void ChairInfo_Read_1()
        {
            var mock = new Mock<ISyncProvider>();
            mock.Setup (c => c.ReadTextFile (It.IsAny<FileSpecification>()))
                .Returns (_GetMenuText());
            var connection = mock.Object;
            var chairs = ChairInfo.Read (connection, "kv.mnu", true);
            Assert.AreEqual (6, chairs.Length);
            Assert.AreEqual ("*", chairs[0].Code);
            Assert.AreEqual ("Все подразделения", chairs[0].Title);
            Assert.AreEqual ("ЧЗ", chairs[^1].Code);
            Assert.AreEqual ("Читальный зал", chairs[^1].Title);
            mock.Verify
                (
                    c => c.ReadTextFile (It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        [ExpectedException (typeof (IndexOutOfRangeException))]
        public void ChairInfo_Read_1a_Exception()
        {
            var mock = new Mock<ISyncProvider>();
            mock.Setup (c => c.ReadTextFile (It.IsAny<FileSpecification>()))
                .Returns (string.Empty);
            var connection = mock.Object;
            ChairInfo.Read (connection, "kv.mnu", true);
        }

        [TestMethod]
        public void ChairInfo_Read_2()
        {
            var mock = new Mock<ISyncProvider>();
            mock.Setup (c => c.ReadTextFile (It.IsAny<FileSpecification>()))
                .Returns (_GetMenuText());
            var connection = mock.Object;
            var chairs = ChairInfo.Read (connection);
            Assert.AreEqual (6, chairs.Length);
            Assert.AreEqual ("*", chairs[0].Code);
            Assert.AreEqual ("Все подразделения", chairs[0].Title);
            Assert.AreEqual ("ЧЗ", chairs[chairs.Length - 1].Code);
            Assert.AreEqual ("Читальный зал", chairs[chairs.Length - 1].Title);
            mock.Verify
                (
                    c => c.ReadTextFile (It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        private void _TestSerialization
            (
                ChairInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ChairInfo>();

            Assert.IsNotNull (second);
            Assert.AreEqual (first.Code, second!.Code);
            Assert.AreEqual (first.Title, second.Title);
        }

        [TestMethod]
        public void ChairInfo_Serialization_1()
        {
            var chair = new ChairInfo();
            _TestSerialization (chair);

            chair.Code = "АБ";
            chair.Title = "Абонемент";
            _TestSerialization (chair);
        }

        [TestMethod]
        public void IsbnInfo_ToXml_1()
        {
            var chair = new ChairInfo();
            Assert.AreEqual ("<chair />", XmlUtility.SerializeShort (chair));

            chair = new ChairInfo ("АБ");
            Assert.AreEqual ("<chair code=\"АБ\" />", XmlUtility.SerializeShort (chair));

            chair = new ChairInfo ("АБ", "Абонемент");
            Assert.AreEqual ("<chair code=\"АБ\" title=\"Абонемент\" />", XmlUtility.SerializeShort (chair));
        }

        [Ignore]
        [TestMethod]
        public void IsbnInfo_ToJson_1()
        {
            var chair = new ChairInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (chair));

            chair = new ChairInfo ("АБ");
            Assert.AreEqual ("{'code':'АБ'}", JsonUtility.SerializeShort (chair));

            chair = new ChairInfo ("АБ", "Абонемент");
            Assert.AreEqual ("{'code':'АБ','title':'Абонемент'}", JsonUtility.SerializeShort (chair));
        }

        [TestMethod]
        public void ChairInfo_ToString_1()
        {
            var chair = new ChairInfo();
            Assert.AreEqual ("(null)", chair.ToString());

            chair = new ChairInfo ("АБ");
            Assert.AreEqual ("АБ", chair.ToString());

            chair = new ChairInfo ("АБ", "Абонемент");
            Assert.AreEqual ("АБ - Абонемент", chair.ToString());
        }
    }
}
