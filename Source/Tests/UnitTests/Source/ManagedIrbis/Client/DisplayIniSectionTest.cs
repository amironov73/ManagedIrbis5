// ReSharper disable IdentifierTypo
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
    public class DisplayIniSectionTest
        : Common.CommonUnitTest
    {
        private string _GetFileName()
        {
            return Path.Combine (Irbis64RootPath, "irbisc.ini");
        }

        private IniFile _GetIniFile()
        {
            return new (_GetFileName(), IrbisEncoding.Ansi);
        }

        [TestMethod]
        public void DisplayIniSection_Construction_1()
        {
            var section = new DisplayIniSection();
            Assert.AreEqual (DisplayIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void DisplayIniSection_Construction_2()
        {
            var iniFile = _GetIniFile();
            var section = new DisplayIniSection (iniFile);
            Assert.AreEqual (DisplayIniSection.SectionName, section.Section.Name);
            Assert.AreSame (iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DisplayIniSection_Construction_3()
        {
            var iniFile = _GetIniFile();
            var iniSection = iniFile.GetSection (DisplayIniSection.SectionName);
            Assert.IsNotNull (iniSection);
            var section = new DisplayIniSection (iniSection);
            Assert.AreSame (iniSection, section.Section);
            Assert.AreEqual (DisplayIniSection.SectionName, section.Section.Name);
            Assert.AreSame (iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DisplayIniSection_MaxBriefPortion_1()
        {
            var section = new DisplayIniSection();
            Assert.AreEqual (6, section.MaxBriefPortion);
            section.MaxBriefPortion = 12345;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual ("[Display]\nMaxBriefPortion=12345\n", actual);
        }

        [TestMethod]
        public void DisplayIniSection_MaxMarked_1()
        {
            var section = new DisplayIniSection();
            Assert.AreEqual (100, section.MaxMarked);
            section.MaxMarked = 12345;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual ("[Display]\nMaxMarked=12345\n", actual);
        }
    }
}
