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
    public class ContextIniSectionTest
        : Common.CommonUnitTest
    {
        private string _GetFileName() => Path.Combine(TestDataPath, "cirbisc.ini");

        private IniFile _GetIniFile() => new (_GetFileName(), IrbisEncoding.Ansi);

        [TestMethod]
        public void ContextIniSection_Construction_1()
        {
            var section = new ContextIniSection();
            Assert.AreEqual(ContextIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void ContextIniSection_Construction_2()
        {
            var iniFile = _GetIniFile();
            var section = new ContextIniSection(iniFile);
            Assert.AreEqual(ContextIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void ContextIniSection_Construction_3()
        {
            var iniFile = _GetIniFile();
            var iniSection = iniFile.GetSection(ContextIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            var section = new ContextIniSection(iniSection!);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(ContextIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void ContextIniSection_Database_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.Database);
            section.Database = "IBIS";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nDBN=IBIS\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_DisplayFormat_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.DisplayFormat);
            section.DisplayFormat = "Оптимизированный";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nPFT=Оптимизированный\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Mfn_1()
        {
            var section = new ContextIniSection();
            Assert.AreEqual(0, section.Mfn);
            section.Mfn = 123;
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nCURMFN=123\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Password_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.Password);
            section.Password = "Пароль";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nUserPassword=Пароль\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Query_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.Query);
            section.Query = "\"T=ЗАГЛАВИЕ\"";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nQUERY=\"T=ЗАГЛАВИЕ\"\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_SearchPrefix_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.SearchPrefix);
            section.SearchPrefix = "T=";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nPREFIX=T=\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_UserName_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.UserName);
            section.UserName = "Пользователь";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nUserName=Пользователь\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Worksheet_1()
        {
            var section = new ContextIniSection();
            Assert.IsNull(section.Worksheet);
            section.Worksheet = "ASP52MRS";
            var actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nWS=ASP52MRS\n", actual);
        }
    }
}
