// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Opt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public sealed class OptLineTest
    {
        private OptLine _GetLine()
        {
            return new ()
            {
                Key = "J",
                Value = "!RPJ51"
            };
        }

        private void _Compare
            (
                OptLine first,
                OptLine second
            )
        {
            Assert.AreEqual (first.Key, second.Key);
            Assert.AreEqual (first.Value, second.Value);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void OptLine_Construction_1()
        {
            var line = new OptLine();
            Assert.IsNull (line.Key);
            Assert.IsNull (line.Value);
        }

        [TestMethod]
        [Description ("Сравнение строки с ключом: пустая строка")]
        [ExpectedException (typeof (ArgumentException))]
        public void OptLine_Compare_1()
        {
            var line = new OptLine();
            line.Compare ("Some");
        }

        [TestMethod]
        [Description ("Сравнение строки с ключом: непустая строка")]
        public void OptLine_Compare_2()
        {
            var line = _GetLine();
            Assert.IsFalse (line.Compare ("Some"));
            Assert.IsTrue (line.Compare ("J"));
        }

        [TestMethod]
        [Description ("Разбор строки: пустая строка")]
        public void OptLine_Parse_1()
        {
            Assert.IsNull (OptLine.Parse (null));
            Assert.IsNull (OptLine.Parse (string.Empty));
        }

        [TestMethod]
        [Description ("Разбор строки: начальные и конечные пробелы")]
        public void OptLine_Parse_2()
        {
            var line = OptLine.Parse ("  Key Value  ");
            Assert.IsNotNull (line);
            Assert.AreEqual ("Key", line.Key);
            Assert.AreEqual ("Value", line.Value);
        }

        [TestMethod]
        [Description ("Разбор строки: неверно сформированная строка")]
        public void OptLine_Parse_3()
        {
            var line = OptLine.Parse ("  Key Value Third ");
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Разбор строки: строка из пробелов")]
        public void OptLine_Parse_4()
        {
            var line = OptLine.Parse ("  ");
            Assert.IsNull (line);
        }

        private void _TestSerialization
            (
                OptLine first
            )
        {
            var memory = first.SaveToMemory();
            var second = memory.RestoreObjectFromMemory<OptLine>();
            Assert.IsNotNull (second);
            _Compare (first, second);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void OptLine_Serialization_1()
        {
            var line = new OptLine();
            _TestSerialization (line);

            line = _GetLine();
            _TestSerialization (line);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void OptLine_Verification_1()
        {
            var line = new OptLine();
            Assert.IsFalse (line.Verify (false));

            line = _GetLine();
            Assert.IsTrue (line.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void OptLine_ToXml_1()
        {
            var line = new OptLine();
            Assert.AreEqual
                (
                    "<line />",
                    XmlUtility.SerializeShort (line)
                );

            line = _GetLine();
            Assert.AreEqual
                (
                    "<line key=\"J\" value=\"!RPJ51\" />",
                    XmlUtility.SerializeShort (line)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void OptLine_ToJson_1()
        {
            var line = new OptLine();
            Assert.AreEqual
                (
                    "{}",
                    JsonUtility.SerializeShort (line)
                );

            line = _GetLine();
            Assert.AreEqual
                (
                    "{\"key\":\"J\",\"value\":\"!RPJ51\"}",
                    JsonUtility.SerializeShort (line)
                );
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void OptLine_ToString_1()
        {
            var line = new OptLine();
            Assert.AreEqual
                (
                    "(null) (null)",
                    line.ToString()
                );

            line = _GetLine();
            Assert.AreEqual
                (
                    "J !RPJ51",
                    line.ToString()
                );
        }
    }
}
