﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.IO;
using AM.Text;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class EntryIniSectionTest
        : Common.CommonUnitTest
    {
        private string _GetFileName() => Path.Combine(Irbis64RootPath, "irbisc.ini");

        private IniFile _GetIniFile() => new (_GetFileName(), IrbisEncoding.Ansi);

        [TestMethod]
        public void EntryIniSection_Construction_1()
        {
            var section = new EntryIniSection();
            Assert.AreEqual(EntryIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void EntryIniSection_Construction_2()
        {
            var iniFile = _GetIniFile();
            var section = new EntryIniSection(iniFile);
            Assert.AreEqual(EntryIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void EntryIniSection_Construction_3()
        {
            var iniFile = _GetIniFile();
            var iniSection = iniFile.GetSection(EntryIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            var section = new EntryIniSection(iniSection!);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(EntryIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void EntryIniSection_DbnFlc_1()
        {
            var section = new EntryIniSection();
            Assert.AreEqual("DBNFLC", section.DbnFlc);
            section.DbnFlc = "12345";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nDBNFLC=12345\n", actual);
        }

        [TestMethod]
        public void EntryIniSection_DefFieldNumb_1()
        {
            var section = new EntryIniSection();
            Assert.AreEqual(10, section.DefFieldNumb);
            section.DefFieldNumb = 12345;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nDefFieldNumb=12345\n", actual);
        }

        [TestMethod]
        public void EntryIniSection_MaxAddFields_1()
        {
            var section = new EntryIniSection();
            Assert.AreEqual(10, section.MaxAddFields);
            section.MaxAddFields = 12345;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nMaxAddFields=12345\n", actual);
        }

        [TestMethod]
        public void EntryIniSection_RecordUpdate_1()
        {
            var section = new EntryIniSection();
            Assert.AreEqual(true, section.RecordUpdate);
            section.RecordUpdate = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nRECUPDIF=0\n", actual);
        }
    }
}
