// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class OrgMnuTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void OrgMnu_Construction_1()
        {
            var org = new OrgMnu();
            Assert.IsNotNull(org.Country);
            Assert.IsNotNull(org.Organization);
            Assert.IsNotNull(org.Currency);
            Assert.IsNotNull(org.Volume);
            Assert.IsNotNull(org.Position);
            Assert.IsNotNull(org.Language);
            Assert.IsNotNull(org.Check);
            Assert.IsNotNull(org.Technology);
            Assert.IsNotNull(org.AuthorSign);
            Assert.IsNotNull(org.ExtendedAuthors);
            Assert.IsNotNull(org.Sigla);
        }

        [TestMethod]
        public void OrgMnu_Construction_2()
        {
            var path = Path.Combine(Irbis64RootPath, "Datai/Deposit/org.mnu");
            var menu = MenuFile.ParseLocalFile(path);
            var org = new OrgMnu(menu);
            Assert.IsNotNull(org.Country);
            Assert.IsNotNull(org.Organization);
            Assert.IsNotNull(org.Currency);
            Assert.IsNotNull(org.Volume);
            Assert.IsNotNull(org.Position);
            Assert.IsNotNull(org.Language);
            Assert.IsNotNull(org.Check);
            Assert.IsNotNull(org.Technology);
            Assert.IsNotNull(org.AuthorSign);
            Assert.IsNotNull(org.ExtendedAuthors);
            Assert.IsNotNull(org.Sigla);
        }

        [TestMethod]
        public void OrgMnu_ApplyToMenu_1()
        {
            var org = new OrgMnu();
            var menu = new MenuFile();
            org.ApplyToMenu(menu);
            Assert.AreEqual(org.Country, menu.GetString("1"));
            Assert.AreEqual(org.Organization, menu.GetString("2"));
            Assert.AreEqual(org.Currency, menu.GetString("3"));
            Assert.AreEqual(org.Volume, menu.GetString("4"));
            Assert.AreEqual(org.Position, menu.GetString("5"));
            Assert.AreEqual(org.Language, menu.GetString("6"));
            Assert.AreEqual(org.Check, menu.GetString("7"));
            Assert.AreEqual(org.Technology, menu.GetString("8"));
            Assert.AreEqual(org.AuthorSign, menu.GetString("9"));
            Assert.AreEqual(org.ExtendedAuthors, menu.GetString("A"));
            Assert.AreEqual(org.Sigla, menu.GetString("S"));
        }
    }
}
