using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Menus;

using Newtonsoft.Json;

// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using static ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void Search_All_1()
        {
            Assert.AreEqual("I=$", All().ToString());
        }

        [TestMethod]
        public void Search_EqualsTo_1()
        {
            var search = EqualsTo("prefix=", "value1");
            Assert.AreEqual("prefix=value1", search.ToString());
        }

        [TestMethod]
        public void Search_EqualsTo_2()
        {
            var search = EqualsTo("prefix=", "value with space");
            Assert.AreEqual("\"prefix=value with space\"", search.ToString());
        }

        [TestMethod]
        public void Search_EqualsTo_3()
        {
            var search = EqualsTo("prefix=", "value1", "value2");
            Assert.AreEqual("prefix=value1 + prefix=value2", search.ToString());
        }

        [TestMethod]
        public void Search_EqualsTo_4()
        {
            var search = EqualsTo("prefix=", "value 1", "value 2");
            Assert.AreEqual("\"prefix=value 1\" + \"prefix=value 2\"", search.ToString());
        }

        [TestMethod]
        public void Search_Keyword_1()
        {
            var search = Keyword("сказки$");
            Assert.AreEqual("K=сказки$", search.ToString());
        }

        [TestMethod]
        public void Search_Keyword_2()
        {
            var search = Keyword("народные сказки$");
            Assert.AreEqual("\"K=народные сказки$\"", search.ToString());
        }

        [TestMethod]
        public void Search_Keyword_3()
        {
            var search = Keyword("сказки", "легенды");
            Assert.AreEqual("K=сказки + K=легенды", search.ToString());
        }

        [TestMethod]
        public void Search_And_1()
        {
            var search = Keyword("сказки").And(Author("Пушкин$"));
            Assert.AreEqual("(K=сказки * A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_And_2()
        {
            var search = Keyword("сказки").And(Author("Пушкин$"), Year("2010"));
            Assert.AreEqual("(K=сказки * A=Пушкин$ * G=2010)", search.ToString());
        }

        [TestMethod]
        public void Search_And_3()
        {
            var search = Keyword("сказки").And("A=Пушкин$", "G=2010");
            Assert.AreEqual("(K=сказки * A=Пушкин$ * G=2010)", search.ToString());
        }

        [TestMethod]
        public void Search_Or_1()
        {
            var search = Keyword("сказки").Or(Author("Пушкин$"));
            Assert.AreEqual("(K=сказки + A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_Or_2()
        {
            var search = Keyword("сказки").Or(Author("Пушкин$"), Year("2010"));
            Assert.AreEqual("(K=сказки + A=Пушкин$ + G=2010)", search.ToString());
        }

        [TestMethod]
        public void Search_Or_3()
        {
            var search = Keyword("сказки").Or("A=Пушкин$", "G=2010");
            Assert.AreEqual("(K=сказки + A=Пушкин$ + G=2010)", search.ToString());
        }

        [TestMethod]
        public void Search_Not_1()
        {
            var search = Keyword("сказки").Not(Author("Пушкин$"));
            Assert.AreEqual("(K=сказки ^ A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_Not_2()
        {
            var search = Keyword("сказки").Not("A=Пушкин$");
            Assert.AreEqual("(K=сказки ^ A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_SameField_1()
        {
            var search = Keyword("сказки").SameField(Author("Пушкин$"));
            Assert.AreEqual("(K=сказки (G) A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_SameField_2()
        {
            var search = Keyword("сказки").SameField("A=Пушкин$");
            Assert.AreEqual("(K=сказки (G) A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_SameRepeat_1()
        {
            var search = Keyword("сказки").SameRepeat(Author("Пушкин$"));
            Assert.AreEqual("(K=сказки (F) A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_SameRepeat_2()
        {
            var search = Keyword("сказки").SameRepeat("A=Пушкин$");
            Assert.AreEqual("(K=сказки (F) A=Пушкин$)", search.ToString());
        }

        [TestMethod]
        public void Search_WrapIfNeeded_1()
        {
            Assert.AreEqual("\"\"", WrapIfNeeded(string.Empty));
            Assert.AreEqual("Hello", WrapIfNeeded("Hello"));
            Assert.AreEqual("\"Hello, world\"", WrapIfNeeded("Hello, world"));
            Assert.AreEqual("\"Hello, world\"", WrapIfNeeded("\"Hello, world\""));
        }

        [TestMethod]
        public void Search_Title_1()
        {
            var search = Title("сказки$");
            Assert.AreEqual("T=сказки$", search.ToString());
        }

        [TestMethod]
        public void Search_Title_2()
        {
            var search = Title("сказки", "легенды");
            Assert.AreEqual("T=сказки + T=легенды", search.ToString());
        }

        [TestMethod]
        public void Search_Publisher_1()
        {
            var search = Publisher("АСТ");
            Assert.AreEqual("O=АСТ", search.ToString());
        }

        [TestMethod]
        public void Search_Publisher_2()
        {
            var search = Publisher("АСТ", "Эксмо");
            Assert.AreEqual("O=АСТ + O=Эксмо", search.ToString());
        }

        [TestMethod]
        public void Search_Place_1()
        {
            var search = Place("Москва");
            Assert.AreEqual("MI=Москва", search.ToString());
        }

        [TestMethod]
        public void Search_Place_2()
        {
            var search = Place("Москва", "Иркутск");
            Assert.AreEqual("MI=Москва + MI=Иркутск", search.ToString());
        }

        [TestMethod]
        public void Search_Subject_1()
        {
            var search = Subject("наука");
            Assert.AreEqual("S=наука", search.ToString());
        }

        [TestMethod]
        public void Search_Subject_2()
        {
            var search = Subject("наука", "техника");
            Assert.AreEqual("S=наука + S=техника", search.ToString());
        }

        [TestMethod]
        public void Search_Language_1()
        {
            var search = Language("rus");
            Assert.AreEqual("J=rus", search.ToString());
        }

        [TestMethod]
        public void Search_Language_2()
        {
            var search = Language("rus", "eng");
            Assert.AreEqual("J=rus + J=eng", search.ToString());
        }

        [TestMethod]
        public void Search_Year_1()
        {
            var search = Year("2010");
            Assert.AreEqual("G=2010", search.ToString());
        }

        [TestMethod]
        public void Search_Year_2()
        {
            var search = Year("2010", "2020");
            Assert.AreEqual("G=2010 + G=2020", search.ToString());
        }

        [TestMethod]
        public void Search_Magazine_1()
        {
            var search = Magazine("Знамя");
            Assert.AreEqual("TJ=Знамя", search.ToString());
        }

        [TestMethod]
        public void Search_Magazine_2()
        {
            var search = Magazine("Знамя", "Власть");
            Assert.AreEqual("TJ=Знамя + TJ=Власть", search.ToString());
        }

        [TestMethod]
        public void Search_DocumentKind_1()
        {
            var search = DocumentKind("01");
            Assert.AreEqual("V=01", search.ToString());
        }

        [TestMethod]
        public void Search_DocumentKind_2()
        {
            var search = DocumentKind("01", "02");
            Assert.AreEqual("V=01 + V=02", search.ToString());
        }

        [TestMethod]
        public void Search_Udc_1()
        {
            var search = Udc("01");
            Assert.AreEqual("U=01", search.ToString());
        }

        [TestMethod]
        public void Search_Udc_2()
        {
            var search = Udc("01", "02");
            Assert.AreEqual("U=01 + U=02", search.ToString());
        }

        [TestMethod]
        public void Search_Bbk_1()
        {
            var search = Bbk("01");
            Assert.AreEqual("BBK=01", search.ToString());
        }

        [TestMethod]
        public void Search_Bbk_2()
        {
            var search = Bbk("01", "02");
            Assert.AreEqual("BBK=01 + BBK=02", search.ToString());
        }

        [TestMethod]
        public void Search_Rzn_1()
        {
            var search = Rzn("01");
            Assert.AreEqual("RZN=01", search.ToString());
        }

        [TestMethod]
        public void Search_Rzn_2()
        {
            var search = Rzn("01", "02");
            Assert.AreEqual("RZN=01 + RZN=02", search.ToString());
        }

        [TestMethod]
        public void Search_Mhr_1()
        {
            var search = Mhr("01");
            Assert.AreEqual("MHR=01", search.ToString());
        }

        [TestMethod]
        public void Search_Mhr_2()
        {
            var search = Mhr("01", "02");
            Assert.AreEqual("MHR=01 + MHR=02", search.ToString());
        }

        [TestMethod]
        public void Search_Number_1()
        {
            var search = Number("01");
            Assert.AreEqual("IN=01", search.ToString());
        }

        [TestMethod]
        public void Search_Number_2()
        {
            var search = Number("01", "02");
            Assert.AreEqual("IN=01 + IN=02", search.ToString());
        }
    }
}