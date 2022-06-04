// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.Collections.Generic;

using ManagedIrbis;
using ManagedIrbis.Catalog;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Client;

[TestClass]
public class RecordStateComparerTest
{
    [TestMethod]
    public void RecordStateComparer_ByMfn_1()
    {
        var first = new RecordState
        {
            Mfn = 123,
            Status = RecordStatus.Last,
            Version = 5
        };

        var second = new RecordState
        {
            Mfn = 123,
            Status = RecordStatus.LogicallyDeleted,
            Version = 6
        };

        IEqualityComparer<RecordState> comparer = new RecordStateComparer.ByMfn();
        Assert.IsTrue (comparer.Equals (first, second));
        Assert.AreEqual
            (
                comparer.GetHashCode (first),
                comparer.GetHashCode (second)
            );

        second.Mfn = 124;
        Assert.IsFalse (comparer.Equals (first, second));
        Assert.AreNotEqual
            (
                comparer.GetHashCode (first),
                comparer.GetHashCode (second)
            );
    }

    [TestMethod]
    public void RecordStateComparer_ByVersion_1()
    {
        var first = new RecordState
        {
            Mfn = 123,
            Status = RecordStatus.Last,
            Version = 5
        };

        var second = new RecordState
        {
            Mfn = 123,
            Status = RecordStatus.LogicallyDeleted,
            Version = 5
        };

        IEqualityComparer<RecordState> comparer = new RecordStateComparer.ByVersion();
        Assert.IsTrue (comparer.Equals (first, second));
        Assert.AreEqual
            (
                comparer.GetHashCode (first),
                comparer.GetHashCode (second)
            );

        second.Mfn = 124;
        Assert.IsFalse (comparer.Equals (first, second));
        Assert.AreNotEqual
            (
                comparer.GetHashCode (first),
                comparer.GetHashCode (second)
            );

        second.Mfn = 123;
        second.Version = 6;
        Assert.IsFalse (comparer.Equals (first, second));
        Assert.AreNotEqual
            (
                comparer.GetHashCode (first),
                comparer.GetHashCode (second)
            );
    }
}
