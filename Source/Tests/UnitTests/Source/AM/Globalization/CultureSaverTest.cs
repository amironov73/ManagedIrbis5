// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

#nullable enable

namespace UnitTests.AM.Globalization
{
    [TestClass]
    public class CultureSaverTest
    {
        [TestMethod]
        public void CultureSaver_ForTesting_1()
        {
            var saved = CultureInfo.CurrentCulture.Name;

            using (CultureSaver.ForTesting())
            {
                Assert.AreEqual (CultureCode.AmericanEnglish, CultureInfo.CurrentCulture.Name);
            }

            Assert.AreEqual (saved, CultureInfo.CurrentCulture.Name);
        }
    }
}
