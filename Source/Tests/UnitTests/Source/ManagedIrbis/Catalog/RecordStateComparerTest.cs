// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Collections.Generic;

using ManagedIrbis;
using ManagedIrbis.Catalog;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Client;

[TestClass]
public class RecordStateComparerTest
{
    [TestMethod]
    [Description ("Сравнение по MFN")]
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
    [Description ("Сравнение по номеру версии")]
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
