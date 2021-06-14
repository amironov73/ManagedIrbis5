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
    public class DesktopIniSectionTest
        : Common.CommonUnitTest
    {
        private string _GetFileName() => Path.Combine(TestDataPath, "cirbisc.ini");

        private IniFile _GetIniFile() => new (_GetFileName(), IrbisEncoding.Ansi);

        [TestMethod]
        public void DesktopIniSection_Construction_1()
        {
            var section = new DesktopIniSection();
            Assert.AreEqual(DesktopIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void DesktopIniSection_Construction_2()
        {
            var iniFile = _GetIniFile();
            var section = new DesktopIniSection(iniFile);
            Assert.AreEqual(DesktopIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DesktopIniSection_Construction_3()
        {
            var iniFile = _GetIniFile();
            var iniSection = iniFile.GetSection(DesktopIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            var section = new DesktopIniSection(iniSection!);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(DesktopIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DesktopIniSection_AutoService_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.AutoService);
            section.AutoService = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nAutoService=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBContext_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.DBContext);
            section.DBContext = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBContext=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBContextFloating_1()
        {
            var section = new DesktopIniSection();
            Assert.IsFalse(section.DBContextFloating);
            section.DBContextFloating = true;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBContextFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBOpen_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.DBOpen);
            section.DBOpen = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBOpen=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBOpenFloating_1()
        {
            var section = new DesktopIniSection();
            Assert.IsFalse(section.DBOpenFloating);
            section.DBOpenFloating = true;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBOpenFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_Entry_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.Entry);
            section.Entry = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nEntry=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_EntryFloating_1()
        {
            var section = new DesktopIniSection();
            Assert.IsFalse(section.EntryFloating);
            section.EntryFloating = true;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nEntryFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_MainMenu_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.MainMenu);
            section.MainMenu = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nMainMenu=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_MainMenuFloating_1()
        {
            var section = new DesktopIniSection();
            Assert.IsFalse(section.MainMenuFloating);
            section.MainMenuFloating = true;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nMainMenuFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_Search_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.Search);
            section.Search = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nSearch=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_SearchFloating_1()
        {
            var section = new DesktopIniSection();
            Assert.IsFalse(section.SearchFloating);
            section.SearchFloating = true;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nSearchFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_Spelling_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.Spelling);
            section.Spelling = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nSpelling=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_UserMode_1()
        {
            var section = new DesktopIniSection();
            Assert.IsTrue(section.UserMode);
            section.UserMode = false;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nUserMode=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_UserModeFloating_1()
        {
            var section = new DesktopIniSection();
            Assert.IsFalse(section.UserModeFloating);
            section.UserModeFloating = true;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nUserModeFloating=1\n", actual);
        }
    }
}
