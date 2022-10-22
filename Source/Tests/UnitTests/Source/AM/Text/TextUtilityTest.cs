// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

namespace UnitTests.AM.Text;

[TestClass]
public sealed class TextUtilityTest
{
    [TestMethod]
    public void TextUtility_DetermineTextKind_1()
    {
        Assert.AreEqual
            (
                TextKind.PlainText,
                TextUtility.DetermineTextKind (null)
            );

        Assert.AreEqual
            (
                TextKind.PlainText,
                TextUtility.DetermineTextKind ("")
            );

        Assert.AreEqual
            (
                TextKind.RichText,
                TextUtility.DetermineTextKind (@"{\rtf1 Hello}")
            );

        Assert.AreEqual
            (
                TextKind.RichText,
                TextUtility.DetermineTextKind (@"Hello, {\b World}!")
            );

        Assert.AreEqual
            (
                TextKind.Html,
                TextUtility.DetermineTextKind ("<html><body>Hello</body></html>")
            );

        Assert.AreEqual
            (
                TextKind.Html,
                TextUtility.DetermineTextKind ("Hello, <b>World</b>!")
            );

        Assert.AreEqual
            (
                TextKind.PlainText,
                TextUtility.DetermineTextKind ("Hello, world!")
            );

        Assert.AreEqual
            (
                TextKind.PlainText,
                TextUtility.DetermineTextKind ("Hello, <world}!")
            );
    }
}
