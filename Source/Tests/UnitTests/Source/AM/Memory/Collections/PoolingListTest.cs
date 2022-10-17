// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace UnitTests.AM.Memory.Collections.Specialized;

[TestClass]
public sealed class PoolingListTests
{
    [TestMethod]
    public void TestEmpty()
    {
        using var list = new PoolingListCanon<object>();

        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (false, list.IsReadOnly);
        Assert.AreEqual (-1, list.IndexOf (1));
    }

    [TestMethod]
    public void SingleAddition()
    {
        using var list = new PoolingListCanon<object>();
        list.Add (30);

        Assert.AreEqual (1, list.Count);
        Assert.AreEqual (30, list[0]);
    }

    [TestMethod]
    public void AllAdditions()
    {
        using var list = new PoolingListCanon<object>();
        for (var i = 0; i < LocalList<int>.LocalStoreCapacity; i++)
        {
            list.Add (i * 10);
        }

        Assert.AreEqual (LocalList<int>.LocalStoreCapacity, list.Count);
    }

    [TestMethod]
    public void SingleAdditionAndRemove()
    {
        using var list = new PoolingListCanon<object>();
        list.Add (30);
        list.Remove (30);

        Assert.AreEqual (0, list.Count);
        Assert.ThrowsException<IndexOutOfRangeException> (() =>
        {
            var _ = list[0];
        });
    }

    [TestMethod]
    public void SingleAdditionAndRemoveAt()
    {
        using var list = new PoolingListCanon<object>();
        list.Add (30);
        list.RemoveAt (0);

        Assert.AreEqual (0, list.Count);
        Assert.ThrowsException<IndexOutOfRangeException> (() =>
        {
            var _ = list[0];
        });
    }

    [TestMethod]
    public void RemoveIfEmpty()
    {
        using var list = new PoolingListCanon<object>();

        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (false, list.Remove (29));
    }

    [TestMethod]
    public void RemoveAtIfEmpty()
    {
        using var list = new PoolingListCanon<object>();

        Assert.AreEqual (0, list.Count);
        Assert.ThrowsException<IndexOutOfRangeException> (() => { list.RemoveAt (0); });
    }

    [TestMethod]
    public void MakeListFromValueTuple()
    {
        using var list = new PoolingListCanon<object>();
        list.Add (10);
        list.Add (20);
        list.Add (30);


        Assert.AreEqual (3, list.Count);
        Assert.AreEqual (10, list[0]);
        Assert.AreEqual (20, list[1]);
        Assert.AreEqual (30, list[2]);

        Assert.ThrowsException<IndexOutOfRangeException> (() =>
        {
            var _ = list[3];
        });
    }

    [TestMethod]
    public void MakeListFromValueTupleAndGoBack()
    {
        using var list = new PoolingListCanon<object>();

        list.Add (10);
        list.Add (20);
        list.Add (30);
        list.Remove (30);

        Assert.AreEqual (2, list.Count);
        Assert.AreEqual (10, list[0]);
        Assert.AreEqual (20, list[1]);

        Assert.ThrowsException<IndexOutOfRangeException> (() =>
        {
            var _ = list[2];
        });
    }

    [TestMethod]
    public void MakeListFromValueTupleClearAndFillBack()
    {
        using var list = new PoolingListCanon<object>();

        list.Add (10);
        list.Add (20);
        list.Add (30);

        list.Clear();

        Assert.AreEqual (0, list.Count);
        Assert.ThrowsException<IndexOutOfRangeException> (() =>
        {
            var _ = list[0];
        });

        list.Add (10);
        list.Add (20);
        list.Add (30);

        Assert.AreEqual (3, list.Count);
        Assert.AreEqual (10, list[0]);
        Assert.AreEqual (20, list[1]);
        Assert.AreEqual (30, list[2]);

        Assert.ThrowsException<IndexOutOfRangeException> (() =>
        {
            var _ = list[3];
        });
    }
}
