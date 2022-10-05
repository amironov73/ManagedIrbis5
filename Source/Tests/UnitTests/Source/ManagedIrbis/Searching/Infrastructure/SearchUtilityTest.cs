// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis.Search;

[TestClass]
public class SearchUtilityTest
{
    [TestMethod]
    public void SearchUtility_EscapeQuotation_1()
    {
        Assert.AreEqual
            (
                null,
                SearchUtility.EscapeQuotation (null)
            );
        Assert.AreEqual
            (
                string.Empty,
                SearchUtility.EscapeQuotation (string.Empty)
            );
        Assert.AreEqual
            (
                "Hello",
                SearchUtility.EscapeQuotation ("Hello")
            );
        Assert.AreEqual
            (
                "<.>Hello<.>",
                SearchUtility.EscapeQuotation ("\"Hello\"")
            );
        Assert.AreEqual
            (
                "<.><.>",
                SearchUtility.EscapeQuotation ("\"\"")
            );
    }

    [TestMethod]
    public void SearchUtility_UnescapeQuotation_1()
    {
        Assert.AreEqual
            (
                null,
                SearchUtility.UnescapeQuotation (null)
            );
        Assert.AreEqual
            (
                string.Empty,
                SearchUtility.UnescapeQuotation (string.Empty)
            );
        Assert.AreEqual
            (
                "Hello",
                SearchUtility.UnescapeQuotation ("Hello")
            );
        Assert.AreEqual
            (
                "\"Hello\"",
                SearchUtility.UnescapeQuotation ("<.>Hello<.>")
            );
        Assert.AreEqual
            (
                "\"\"",
                SearchUtility.UnescapeQuotation ("<.><.>")
            );
    }
}
