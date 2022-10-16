// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

#endregion

#nullable enable

namespace UnitTests.AM.Globalization;

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
