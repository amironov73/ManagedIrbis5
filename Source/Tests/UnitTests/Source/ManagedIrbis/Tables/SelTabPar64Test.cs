// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Tables;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Tables
{
    [TestClass]
    public sealed class SelTabPar64Test
        : Common.CommonUnitTest
    {
        private SelTabPar64 _GetSelTab()
        {
            return new ()
            {
                Tables =
                {
                    new ()
                    {
                        TableName = "The table",
                        SelectionMethod = 3,
                        Worksheet = "atw.wss",
                        Format = "@brief",
                        Filter = "v10:'qq'",
                        ModelField = "v20"
                    }
                }
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

        private void _Compare
            (
                SelTabPar64 first,
                SelTabPar64 second
            )
        {
            Assert.AreEqual (first.Tables.Count, second.Tables.Count);
            for (int i = 0; i < first.Tables.Count; i++)
            {
                _Compare (first.Tables[i], second.Tables[i]);
            }
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void SelTabPar64_Construction_1()
        {
            var selTab = new SelTabPar64();
            Assert.IsNotNull (selTab.Tables);
            Assert.AreEqual (0, selTab.Tables.Count);
        }

        [TestMethod]
        [Description ("Чтение из текстового потока")]
        public void SelTabPar64_ParseStream_1()
        {
            var selTab = SelTabPar64.ParseStream (StreamReader.Null);
            Assert.IsNotNull (selTab);
            Assert.IsNotNull (selTab.Tables);
            Assert.AreEqual (0, selTab.Tables.Count);
        }

        [TestMethod]
        [Description ("Чтение из текстового потока")]
        public void SelTabPar64_ParseStream_2()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "Irbis64",
                    "Datai",
                    "CMPL",
                    "seltab64.par"
                );
            var content = File.ReadAllText (fileName);
            using var reader = new StringReader (content);
            var selTab = SelTabPar64.ParseStream (reader);
            Assert.IsNotNull (selTab);
            Assert.IsNotNull (selTab.Tables);
            Assert.AreEqual (3, selTab.Tables.Count);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void SelTabPar64_Construction_2()
        {
            var selTab = new SelTabPar64
            {
                Tables =
                {
                    new AcquisitionTable()
                    {
                        TableName = "The table",
                        SelectionMethod = 3,
                        Worksheet = "atw.wss",
                        Format = "@brief",
                        Filter = "v10:'qq'",
                        ModelField = "v20"
                    }
                }
            };
            Assert.AreEqual (1, selTab.Tables.Count);
        }

        private void _TestSerialization
            (
                SelTabPar64 first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<SelTabPar64>();
            Assert.IsNotNull (second);
            _Compare (first, second);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void SelTabPar64_Serialization_1()
        {
            var selTab = new SelTabPar64();
            _TestSerialization (selTab);

            selTab = _GetSelTab();
            _TestSerialization (selTab);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void SelTabPar64_Verify_1()
        {
            var selTab = new SelTabPar64();
            Assert.IsFalse (selTab.Verify (false));

            selTab = _GetSelTab();
            Assert.IsTrue (selTab.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void SelTabPar64_ToXml_1()
        {
            var selTab = new SelTabPar64();
            Assert.AreEqual
                (
                    "<selTabPar />",
                    XmlUtility.SerializeShort (selTab)
                );

            selTab = _GetSelTab();
            Assert.AreEqual
                (
                    "<selTabPar><table selectionMethod=\"3\"><name>The table</name><worksheet>atw.wss</worksheet><format>@brief</format><filter>v10:'qq'</filter><modelField>v20</modelField></table></selTabPar>",
                    XmlUtility.SerializeShort (selTab)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void SelTabPar64_ToJson_1()
        {
            var selTab = new SelTabPar64();
            Assert.AreEqual
                (
                    "{\"tables\":[]}",
                    JsonUtility.SerializeShort (selTab)
                );

            selTab = _GetSelTab();
            var expected = "{\"tables\":[{\"name\":\"The table\",\"selectionMethod\":3,\"worksheet\":\"atw.wss\",\"format\":\"@brief\",\"filter\":\"v10:\\u0027qq\\u0027\",\"modelField\":\"v20\"}]}";
            var actual = JsonUtility.SerializeShort (selTab);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void SelTabPar64_ToString_1()
        {
            var selTab = new SelTabPar64();
            Assert.AreEqual
                (
                    "Tables: 0",
                    selTab.ToString()
                );

            selTab = _GetSelTab();
            Assert.AreEqual
                (
                    "Tables: 1",
                    selTab.ToString()
                );
        }

    }
}
