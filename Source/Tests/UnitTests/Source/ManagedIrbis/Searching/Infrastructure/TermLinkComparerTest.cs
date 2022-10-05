// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

using System.Collections.Generic;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Search.Infrastructure;

[TestClass]
public sealed class TermLinkComparerTest
{
    [TestMethod]
    public void TermLinkComparer_ByMfn_1()
    {
        var left = new TermLink { Mfn = 10 };
        var right = new TermLink { Mfn = 10 };
        var comparer = new TermLinkComparer.ByMfn();
        Assert.IsTrue (comparer.Equals (left, right));

        right = new TermLink { Mfn = 11 };
        Assert.IsFalse (comparer.Equals (left, right));
    }

    [TestMethod]
    public void TermLinkComparer_ByTag_1()
    {
        var left = new TermLink { Mfn = 10, Tag = 100 };
        var right = new TermLink { Mfn = 10, Tag = 100 };
        var comparer = new TermLinkComparer.ByTag();
        Assert.IsTrue (comparer.Equals (left, right));

        right = new TermLink { Mfn = 10, Tag = 101 };
        Assert.IsFalse (comparer.Equals (left, right));

        right = new TermLink { Mfn = 11, Tag = 100 };
        Assert.IsFalse (comparer.Equals (left, right));
    }

    [TestMethod]
    public void TermLinkComparer_ByOccurrence_1()
    {
        var left = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1 };
        var right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1 };
        var comparer = new TermLinkComparer.ByOccurrence();
        Assert.IsTrue (comparer.Equals (left, right));

        right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 2 };
        Assert.IsFalse (comparer.Equals (left, right));

        right = new TermLink { Mfn = 10, Tag = 101, Occurrence = 1 };
        Assert.IsFalse (comparer.Equals (left, right));

        right = new TermLink { Mfn = 11, Tag = 100, Occurrence = 1 };
        Assert.IsFalse (comparer.Equals (left, right));
    }

    [TestMethod]
    public void TermLinkComparer_ByIndex_1()
    {
        var left = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1, Index = 2 };
        var right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1, Index = 3 };
        var comparer = new TermLinkComparer.ByIndex();
        Assert.IsTrue (comparer.Equals (left, right));

        right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1, Index = 4 };
        Assert.IsFalse (comparer.Equals (left, right));

        right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 2, Index = 2 };
        Assert.IsFalse (comparer.Equals (left, right));

        right = new TermLink { Mfn = 10, Tag = 101, Occurrence = 1, Index = 2 };
        Assert.IsFalse (comparer.Equals (left, right));

        right = new TermLink { Mfn = 11, Tag = 100, Occurrence = 1, Index = 2 };
        Assert.IsFalse (comparer.Equals (left, right));
    }
}
