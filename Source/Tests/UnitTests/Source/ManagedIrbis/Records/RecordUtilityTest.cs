// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis;
using ManagedIrbis.Records;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Records
{
    [TestClass]
    public sealed class RecordUtilityTest
    {
        [TestMethod]
        [Description ("Пустой формат")]
        public void RecordUtility_SimpleFormat_1()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = string.Empty;
            var actual = record.SimpleFormat (format);
            Assert.IsNull (actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_2()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "v910^b+|,|";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001,N002,N003", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_3()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "\"Наличие\" d910^b";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Наличие", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_4()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "\"Наличие\" d910";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Наличие", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_5()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "\"Отсутствие\" n910^a";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Отсутствие", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_6()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "\"Отсутствие\" n911";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Отсутствие", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_7()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "\"Номера: \" v910^b+|,|";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Номера: N001,N002,N003", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_8()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "v910^b + |,| \" - вот такие номера\" ";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001,N002,N003 - вот такие номера", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_9()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "v910^b |,|";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001,N002,N003,", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_10()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "|+| v910^b";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("+N001+N002+N003", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_11()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "|+| + v910^b";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001+N002+N003", actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_12()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "v912";
            var actual = record.SimpleFormat (format);
            Assert.IsNull (actual);
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_13()
        {
            var record = new Record();
            var output = new StringWriter();
            Assert.IsFalse (record.SimpleFormat (output, null));
        }

        [TestMethod]
        [ExpectedException (typeof (FormatException))]
        public void RecordUtility_SimpleFormat_14()
        {
            var record = new Record();
            var output = new StringWriter();
            Assert.IsFalse (record.SimpleFormat (output, "t1000"));
        }

        [TestMethod]
        public void RecordUtility_SimpleFormat_15()
        {
            var record = new Record()
                .Add (910, 'b', "N001");
            var format = "v910 ^b";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001", actual);
        }

        [TestMethod]
        [ExpectedException (typeof (FormatException))]
        public void RecordUtility_SimpleFormat_16()
        {
            var record = new Record();
            var output = new StringWriter();
            Assert.IsFalse (record.SimpleFormat (output, "v910 hello"));
        }

        [TestMethod]
        [Description ("Суффикс у команды N")]
        public void RecordUtility_SimpleFormat_17()
        {
            var record = new Record()
                .Add (910, 'b', "N001");
            var format = "n911 \"none\"";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("none", actual);
        }

        [TestMethod]
        [Description ("Префикс у команды D")]
        public void RecordUtility_SimpleFormat_18()
        {
            var record = new Record()
                .Add (910, 'b', "N001");
            var format = "d910 \"have\"";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("have", actual);
        }

        [TestMethod]
        [Description ("Пробелы в начале не влияют на результат расформатирования")]
        public void RecordUtility_SimpleFormat_19()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "   v910^b + |,|";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001,N002,N003", actual);
        }

        [TestMethod]
        [Description ("Пробелы в конце не влияют на результат расформатирования")]
        public void RecordUtility_SimpleFormat_20()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = "v910^b + |,|   ";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("N001,N002,N003", actual);
        }

        [TestMethod]
        [Description ("Простой литерал")]
        public void RecordUtility_SimpleFormat_21()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = " 'Hello' ";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Hello", actual);
        }

        [TestMethod]
        [Description ("Формат из одной запятой")]
        public void RecordUtility_SimpleFormat_22()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = ",";
            var actual = record.SimpleFormat (format);
            Assert.IsNull (actual);
        }

        [TestMethod]
        [Description("Составной формат")]
        public void RecordUtility_SimpleFormat_23()
        {
            var record = new Record()
                .Add (910, 'b', "N001")
                .Add (910, 'b', "N002")
                .Add (910, 'b', "N003");
            var format = " 'Hello' ' ' 'World' ";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Hello World", actual);
        }

        [TestMethod]
        [Description("Составной формат")]
        public void RecordUtility_SimpleFormat_24()
        {
            var record = new Record()
                .Add (100, 'a', "Hello")
                .Add (200, 'b', ", ")
                .Add (300, "World");
            var format = " v100^a, v200^b, v300 ";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Hello, World", actual);
        }

        [TestMethod]
        [Description("Составной формат")]
        public void RecordUtility_SimpleFormat_25()
        {
            var record = new Record()
                .Add (100, 'a', "Hello")
                .Add (200, 'b', "World")
                .Add (300, "World");
            var format = " v100^a\", \", v200^b";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Hello, World", actual);
        }

        [TestMethod]
        [Description("Составной формат")]
        public void RecordUtility_SimpleFormat_26()
        {
            var record = new Record()
                .Add (200, 'a', "Ruslan and Ludmila",
                    'e', "poem", 'f', "A. Pushkin");
            var format = " v200^a, \" : \"v200^e, \" / \"v200^f, \" ; \"v200^g ";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Ruslan and Ludmila : poem / A. Pushkin", actual);
        }

        [TestMethod]
        [Description ("Метка поля - не число")]
        [ExpectedException (typeof (FormatException))]
        public void RecordUtility_SimpleFormat_27()
        {
            var record = new Record();
            var format = "vabc^d";
            record.SimpleFormat (format);
        }

        [TestMethod]
        [Description ("Запятая после повторяющегося литерала")]
        public void RecordUtility_SimpleFormat_28()
        {
            var record = new Record()
                .Add (100, 'a', "Hello")
                .Add (200, 'b', "World");
            var format = "v100^a| |, v200^b";
            var actual = record.SimpleFormat (format);
            Assert.AreEqual ("Hello World", actual);
        }

        [TestMethod]
        [Description ("Плюсик без повторяющегося литерала")]
        [ExpectedException (typeof (FormatException))]
        public void RecordUtility_SimpleFormat_29()
        {
            var record = new Record();
            var format = "+ v200^a";
            record.SimpleFormat (format);
        }

        [TestMethod]
        [Description ("Плюсик без повторяющегося литерала")]
        [ExpectedException (typeof (FormatException))]
        public void RecordUtility_SimpleFormat_30()
        {
            var record = new Record();
            var format = "v200^a +";
            record.SimpleFormat (format);
        }

    }
}
