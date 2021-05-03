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
    public class BuiltinCulturesTest
    {
        [TestMethod]
        public void BuiltinCultures_AmericanEnglish_1()
        {
            CultureInfo culture = BuiltinCultures.AmericanEnglish;
            Assert.IsNotNull(culture);
            Assert.AreEqual(CultureCode.AmericanEnglish, culture.Name);
        }

        [TestMethod]
        public void BuiltinCultures_Russian_1()
        {
            CultureInfo culture = BuiltinCultures.Russian;
            Assert.IsNotNull(culture);
            Assert.AreEqual(CultureCode.Russian, culture.Name);
        }
    }
}
