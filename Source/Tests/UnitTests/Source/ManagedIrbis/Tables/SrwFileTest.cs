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
    public sealed class SrwFileTest
    {
        private SrwFile _GetSrw()
        {
            return new ()
            {
                Keys =
                {
                    new ()
                    {
                        Length = 100,
                        Multiple = true,
                        Format = "@brief",
                    }
                }
            };
        }

        private void _Compare
            (
                KeyDefinition first,
                KeyDefinition second
            )
        {
            Assert.AreEqual (first.Length, second.Length);
            Assert.AreEqual (first.Multiple, second.Multiple);
            Assert.AreEqual (first.Format, second.Format);
        }

        private void _Compare
            (
                SrwFile first,
                SrwFile second
            )
        {
            Assert.AreEqual (first.Keys.Count, second.Keys.Count);
            for (int i = 0; i < first.Keys.Count; i++)
            {
                _Compare (first.Keys[i], second.Keys[i]);
            }
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void SrwFile_Construction_1()
        {
            var srw = new SrwFile();
            Assert.IsNotNull (srw.Keys);
            Assert.AreEqual (0, srw.Keys.Count);
            Assert.IsNull (srw.UserData);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void SrwFile_Construction_2()
        {
            var srw = new SrwFile()
            {
                Keys =
                {
                    new KeyDefinition()
                }
            };
            Assert.IsNotNull (srw.Keys);
            Assert.AreEqual (1, srw.Keys.Count);
            Assert.IsNull (srw.UserData);
        }

        private void _TestSerialization
            (
                SrwFile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<SrwFile>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void SrwFile_Serialization_1()
        {
            var srw = new SrwFile();
            _TestSerialization (srw);

            srw = _GetSrw();
            srw.UserData = "User data";
            _TestSerialization (srw);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void SrwFile_Verify_1()
        {
            var srw = new SrwFile();
            Assert.IsFalse (srw.Verify (false));

            srw = _GetSrw();
            Assert.IsTrue (srw.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void SrwFile_ToXml_1()
        {
            var srw = new SrwFile();
            Assert.AreEqual
                (
                    "<sorting />",
                    XmlUtility.SerializeShort (srw)
                );

            srw = _GetSrw();
            Assert.AreEqual
                (
                    "<sorting><key length=\"100\" multiple=\"true\"><format>@brief</format></key></sorting>",
                    XmlUtility.SerializeShort (srw)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void SrwFile_ToJson_1()
        {
            var srw = new SrwFile();
            Assert.AreEqual
                (
                    "{\"keys\":[]}",
                    JsonUtility.SerializeShort (srw)
                );

            srw = _GetSrw();
            var expected = "{\"keys\":[{\"length\":100,\"multiple\":true,\"format\":\"@brief\"}]}";
            var actual = JsonUtility.SerializeShort (srw);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void SrwFile_ToString_1()
        {
            var srw = new SrwFile();
            Assert.AreEqual
                (
                    "Keys: 0",
                    srw.ToString()
                );

            srw = _GetSrw();
            Assert.AreEqual
                (
                    "Keys: 1",
                    srw.ToString()
                );
        }

    }
}
