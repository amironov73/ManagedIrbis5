// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

#endregion

#nullable enable

namespace UnitTests.AM.Globalization;

[TestClass]
public class BuiltinCulturesTest
{
    [TestMethod]
    public void BuiltinCultures_AmericanEnglish_1()
    {
        var culture = BuiltinCultures.AmericanEnglish;
        Assert.IsNotNull (culture);
        Assert.AreEqual (CultureCode.AmericanEnglish, culture.Name);
    }

    [TestMethod]
    public void BuiltinCultures_Russian_1()
    {
        var culture = BuiltinCultures.Russian;
        Assert.IsNotNull (culture);
        Assert.AreEqual (CultureCode.Russian, culture.Name);
    }
}
