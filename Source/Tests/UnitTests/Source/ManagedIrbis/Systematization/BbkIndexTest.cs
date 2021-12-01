// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public class BbkIndexTest
    {
        private BbkIndex _GetBbk()
        {
            return new ()
            {
                MainIndex = "11",
                TerritorialIndex = "22",
                CombinedIndex = "33",
                Comma = "44",
                Hren = "55",
                SocialIndex = "66",
                SpecialIndex = { "77" },
                Qualifiers = { "88" }
            };
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BbkIndex_Construction_1()
        {
            var bbk = new BbkIndex();
            Assert.IsNull (bbk.MainIndex);
            Assert.IsNull (bbk.TerritorialIndex);
            Assert.IsNotNull (bbk.SpecialIndex);
            Assert.IsNull (bbk.SocialIndex);
            Assert.IsNull (bbk.CombinedIndex);
            Assert.IsNotNull (bbk.Qualifiers);
            Assert.IsNull (bbk.Hren);
            Assert.IsNull (bbk.Comma);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void BbkIndex_Construction_2()
        {
            var bbk = _GetBbk();
            Assert.AreEqual ("11", bbk.MainIndex);
            Assert.AreEqual ("22", bbk.TerritorialIndex);
            Assert.AreEqual ("33", bbk.CombinedIndex);
            Assert.AreEqual ("44", bbk.Comma);
            Assert.AreEqual ("55", bbk.Hren);
            Assert.AreEqual ("66", bbk.SocialIndex);
            Assert.AreEqual ("77", bbk.SpecialIndex[0]);
            Assert.AreEqual ("88", bbk.Qualifiers[0]);
        }

        [TestMethod]
        public void BbkIndex_Parse_1()
        {
            var bbk = BbkIndex.Parse ("32.973.26-018.2я75");
            Assert.AreEqual (bbk.MainIndex, "32.973.26");
            Assert.AreEqual (bbk.Qualifiers[0], "я75");
            Assert.AreEqual (bbk.SpecialIndex[0], "-018.2");
        }

        [TestMethod]
        public void BbkIndex_Parse_2()
        {
            var bbk = BbkIndex.Parse ("32.973-018.2я7");
            Assert.AreEqual (bbk.MainIndex, "32.973");
            Assert.AreEqual (bbk.Qualifiers[0], "я7");
            Assert.AreEqual (bbk.SpecialIndex[0], "-018.2");
        }

        [TestMethod]
        public void BbkIndex_Parse_3()
        {
            var bbk = BbkIndex.Parse ("22.174");
            Assert.AreEqual (bbk.MainIndex, "22.174");
        }

        [TestMethod]
        public void BbkIndex_Parse_4()
        {
            var bbk = BbkIndex.Parse ("63.3(2Р-2СПб)");
            Assert.AreEqual (bbk.MainIndex, "63.3");
            Assert.AreEqual (bbk.TerritorialIndex, "(2Р-2СПб)");
        }

        [TestMethod]
        public void BbkIndex_Parse_5()
        {
            var bbk = BbkIndex.Parse ("34.621-52.004.05-049.002,27-02(2)к6");
            Assert.AreEqual (bbk.MainIndex, "34.621");
            Assert.AreEqual (bbk.SpecialIndex[0], "-52.004.05");
            Assert.AreEqual (bbk.SpecialIndex[1], "-049.002");
            Assert.AreEqual (bbk.SpecialIndex[2], "-02");
            Assert.AreEqual (bbk.Qualifiers[0], "к6");
            Assert.AreEqual (bbk.TerritorialIndex, "(2)");
        }

        [TestMethod]
        [Description ("Дамп")]
        public void BbkIndex_Dump_1()
        {
            var bbk = _GetBbk();
            var writer = new StringWriter();
            bbk.Dump (writer, ">");
            Assert.AreEqual
                (
                    ">Основной индекс: 11\n>Комбинированный индекс: 33\n>Территориальное деление: 22\n>Некая хрень: 55\n>Запятая: 44\n>Определитель: 88\n>Специальное деление: 77\n>Социальная система : 66\n",
                    writer.ToString().DosToUnix()
                );
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void BbkIndex_ToString_1()
        {
            var bbk = new BbkIndex() { MainIndex = "34" };
            Assert.AreEqual ("34", bbk.ToString());
        }
    }
}
