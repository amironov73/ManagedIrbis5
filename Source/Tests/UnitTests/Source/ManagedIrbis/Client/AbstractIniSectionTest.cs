// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM.IO;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public sealed class AbstractIniSectionTest
    {
        class MyIniSection : AbstractIniSection
        {
            public const string SectionName = "MySection";

            public MyIniSection()
                : base (SectionName)
            {
            }

            public MyIniSection(IniFile iniFile, string sectionName)
                : base(iniFile, sectionName)
            {
            }

            public MyIniSection(string sectionName)
                : base(sectionName)
            {
            }

            public MyIniSection(IniFile.Section section)
                : base(section)
            {
            }
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AbstractIniSection_Construction_1()
        {
            using var section = new MyIniSection();
            Assert.AreEqual (MyIniSection.SectionName, section.Section.Name);
            Assert.AreEqual (0, section.Section.Count);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void AbstractIniSection_Construction_2()
        {
            using var iniFile = new IniFile();
            using var section = new MyIniSection (iniFile, MyIniSection.SectionName);
            Assert.AreEqual (MyIniSection.SectionName, section.Section.Name);
            Assert.AreEqual (0, section.Section.Count);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void AbstractIniSection_Construction_3()
        {
            using var section = new MyIniSection (MyIniSection.SectionName);
            Assert.AreEqual (MyIniSection.SectionName, section.Section.Name);
            Assert.AreEqual (0, section.Section.Count);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void AbstractIniSection_Construction_4()
        {
            using var iniFile = new IniFile();
            using var section = new MyIniSection (iniFile.CreateSection (MyIniSection.SectionName));
            Assert.AreEqual (MyIniSection.SectionName, section.Section.Name);
            Assert.AreEqual (0, section.Section.Count);
        }

        [TestMethod]
        [Description ("Очистка")]
        public void AbstractIniSection_Clear_1()
        {
            var section = new MyIniSection();
            Assert.AreEqual (0, section.Section.Count);
            section.Section.Add ("key", "value");
            Assert.AreEqual (1, section.Section.Count);
            section.Clear();
            Assert.AreEqual (0, section.Section.Count);
            section.Dispose();
        }

        [TestMethod]
        [Description ("Булево значение")]
        public void AbstractIniSection_Boolean_1()
        {
            var name = "Name";
            var section = new MyIniSection();
            Assert.IsFalse (section.GetBoolean (name, "0"));
            section.SetBoolean (name, true);
            Assert.IsTrue (section.GetBoolean (name, "0"));
            section.SetBoolean (name, false);
            Assert.IsFalse (section.GetBoolean (name, "0"));
            section.Dispose();
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void AbstractIniSection_ToString_1()
        {
            var section = new MyIniSection();
            section.Section.Add ("name1", "value1");
            section.Section.Add ("name2", "value2");
            Assert.AreEqual
                (
                    "[MySection]\nname1=value1\nname2=value2\n",
                    section.ToString().DosToUnix()
                );
        }
    }
}
