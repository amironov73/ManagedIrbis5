// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Tables;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Tables
{
    [TestClass]
    public sealed class AcquisitionTableTest
    {
        private AcquisitionTable _GetTable()
        {
            return new ()
            {
                TableName = "The table",
                SelectionMethod = 3,
                Worksheet = "atw.wss",
                Format = "@brief",
                Filter = "v10:'qq'",
                ModelField = "v20"
            };
        }

        private void _Compare
            (
                AcquisitionTable first,
                AcquisitionTable second
            )
        {
            Assert.AreEqual (first.TableName, second.TableName);
            Assert.AreEqual (first.SelectionMethod, second.SelectionMethod);
            Assert.AreEqual (first.Worksheet, second.Worksheet);
            Assert.AreEqual (first.Format, second.Format);
            Assert.AreEqual (first.Filter, second.Filter);
            Assert.AreEqual (first.ModelField, second.ModelField);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AcquisitionTable_Construction_1()
        {
            var table = new AcquisitionTable();
            Assert.IsNull (table.TableName);
            Assert.AreEqual (0, table.SelectionMethod);
            Assert.IsNull (table.Worksheet);
            Assert.IsNull (table.Format);
            Assert.IsNull (table.Filter);
            Assert.IsNull (table.ModelField);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void AcquisitionTable_Construction_2()
        {
            var table = new AcquisitionTable
            {
                TableName = "The table",
                SelectionMethod = 3,
                Worksheet = "atw.wss",
                Format = "@brief",
                Filter = "v10:'qq'",
                ModelField = "v20"
            };
            Assert.AreEqual ("The table", table.TableName);
            Assert.AreEqual (3, table.SelectionMethod);
            Assert.AreEqual ("atw.wss", table.Worksheet);
            Assert.AreEqual ("@brief", table.Format);
            Assert.AreEqual ("v10:'qq'", table.Filter);
            Assert.AreEqual ("v20", table.ModelField);
        }

        private void _TestSerialization
            (
                AcquisitionTable first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<AcquisitionTable>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void AcquisitionTable_Serialization_1()
        {
            var table = new AcquisitionTable();
            _TestSerialization (table);

            table = _GetTable();
            table.UserData = "User data";
            _TestSerialization (table);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void AcquisitionTable_Verify_1()
        {
            var table = new AcquisitionTable();
            Assert.IsFalse (table.Verify (false));

            table = _GetTable();
            Assert.IsTrue (table.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void AcquisitionTable_ToXml_1()
        {
            var table = new AcquisitionTable();
            Assert.AreEqual
                (
                    "<table selectionMethod=\"0\" />",
                    XmlUtility.SerializeShort (table)
                );

            table = _GetTable();
            Assert.AreEqual
                (
                    "<table selectionMethod=\"3\"><name>The table</name><worksheet>atw.wss</worksheet><format>@brief</format><filter>v10:'qq'</filter><modelField>v20</modelField></table>",
                    XmlUtility.SerializeShort (table)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void AcquisitionTable_ToJson_1()
        {
            var table = new AcquisitionTable();
            Assert.AreEqual
                (
                    "{\"selectionMethod\":0}",
                    JsonUtility.SerializeShort (table)
                );

            table = _GetTable();
            var expected = "{\"name\":\"The table\",\"selectionMethod\":3,\"worksheet\":\"atw.wss\",\"format\":\"@brief\",\"filter\":\"v10:\\u0027qq\\u0027\",\"modelField\":\"v20\"}";
            var actual = JsonUtility.SerializeShort (table);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void AcquisitionTable_ToString_1()
        {
            var table = new AcquisitionTable();
            Assert.AreEqual
                (
                    "(null)",
                    table.ToString()
                );

            table = _GetTable();
            Assert.AreEqual
                (
                    "The table",
                    table.ToString()
                );
        }
    }
}
