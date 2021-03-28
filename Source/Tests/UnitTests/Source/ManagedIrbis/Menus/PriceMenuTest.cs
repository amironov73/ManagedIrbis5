// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class PriceMenuTest
        : Common.CommonUnitTest
    {
        private string _GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "Irbis64/Datai/Deposit",
                    PriceMenu.DefaultFileName
                );
        }

        private string _GetContent()
        {
            return File.ReadAllText(_GetFileName(), IrbisEncoding.Ansi);
        }

        private void _CheckMenu
            (
                PriceMenu menu
            )
        {
            Assert.AreEqual(99, menu.Items.Count);
            Assert.AreEqual("1980", menu.Items[0].Date);
        }

        [TestMethod]
        public void ReturnMnu_FromFile_1()
        {
            string fileName = _GetFileName();
            PriceMenu menu = PriceMenu.FromFile(fileName);
            _CheckMenu(menu);
        }

    }
}
