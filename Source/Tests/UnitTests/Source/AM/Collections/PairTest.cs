// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable RedundantCast
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable UnusedVariable
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.Collections;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class PairTest
{
    [TestMethod]
    public void Pair_Construction_1()
    {
        var pair = new Pair<int, string>();
        Assert.AreEqual (0, pair.First);
        Assert.AreEqual (null, pair.Second);
    }

    [TestMethod]
    public void Pair_Construction_2()
    {
        var firstPair = new Pair<int, string> (1, "Hello");
        var secondPair = new Pair<int, string> (firstPair);
        Assert.AreEqual (firstPair.First, secondPair.First);
        Assert.AreEqual (firstPair.Second, secondPair.Second);
    }

    [TestMethod]
    public void Pair_Construction_3()
    {
        var pair = new Pair<int, string> (1);
        Assert.AreEqual (1, pair.First);
        Assert.AreEqual (null, pair.Second);
    }

    [TestMethod]
    public void Pair_Construction_4()
    {
        var pair = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (1, pair.First);
        Assert.AreEqual ("Hello", pair.Second);
    }

    [TestMethod]
    public void Pair_Construction_5()
    {
        var pair = new Pair<int, string> (1, "Hello", true);
        Assert.AreEqual (1, pair.First);
        Assert.AreEqual ("Hello", pair.Second);
        Assert.AreEqual (true, pair.ReadOnly);
    }

    [TestMethod]
    [ExpectedException (typeof (NotSupportedException))]
    public void Pair_Add_1()
    {
        IList pair = new Pair<int, string>();
        pair.Add ("hello");
    }

    [TestMethod]
    public void Pair_Contains_1()
    {
        IList pair = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (true, pair.Contains (1));
        Assert.AreEqual (true, pair.Contains ("Hello"));
        Assert.AreEqual (false, pair.Contains ("World"));
    }

    [TestMethod]
    [ExpectedException (typeof (NotSupportedException))]
    public void Pair_Clear_1()
    {
        IList pair = new Pair<int, string>();
        pair.Clear();
    }

    [TestMethod]
    public void Pair_IndexOf_1()
    {
        IList pair = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (0, pair.IndexOf (1));
        Assert.AreEqual (1, pair.IndexOf ("Hello"));
        Assert.AreEqual (-1, pair.IndexOf ("World"));
    }

    [TestMethod]
    [ExpectedException (typeof (NotSupportedException))]
    public void Pair_Insert_1()
    {
        IList pair = new Pair<int, string>();
        pair.Insert (0, 1);
    }

    [TestMethod]
    [ExpectedException (typeof (NotSupportedException))]
    public void Pair_Remove_1()
    {
        IList pair = new Pair<int, string>();
        pair.Remove (1);
    }

    [TestMethod]
    [ExpectedException (typeof (NotSupportedException))]
    public void Pair_RemoveAt_1()
    {
        IList pair = new Pair<int, string>();
        pair.RemoveAt (1);
    }

    [TestMethod]
    public void Pair_Indexer_1()
    {
        IList pair = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (1, pair[0]);
        Assert.AreEqual ("Hello", pair[1]);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void Pair_Indexer_2()
    {
        IList pair = new Pair<int, string> (1, "Hello");
        object o = pair[2]!;
    }

    [TestMethod]
    public void Pair_Indexer_3()
    {
        var pair = new Pair<int, string> { [0] = 1 };
        Assert.AreEqual (1, pair.First);
        pair[1] = "Hello";
        Assert.AreEqual ("Hello", pair.Second);
    }

    [TestMethod]
    [ExpectedException (typeof (InvalidCastException))]
    public void Pair_Indexer_4()
    {
        var pair = new Pair<int, string> { [0] = "Hello" };
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void Pair_Indexer_5()
    {
        IList pair = new Pair<int, string> (1, "Hello");
        pair[2] = null;
    }

    [TestMethod]
    [ExpectedException (typeof (NotSupportedException))]
    public void Pair_Indexer_6()
    {
        IList pair = new Pair<int, string> (1, "Hello", true);
        pair[0] = 2;
    }

    [TestMethod]
    public void Pair_IsReadOnly_1()
    {
        IList pair = new Pair<int, string>();
        Assert.AreEqual (false, pair.IsReadOnly);

        pair = new Pair<int, string> (1, "Hello", true);
        Assert.AreEqual (true, pair.IsReadOnly);
    }

    [TestMethod]
    public void Pair_IsFixedSize_1()
    {
        IList pair = new Pair<int, string>();
        Assert.AreEqual (true, pair.IsFixedSize);
    }

    [TestMethod]
    public void Pair_CopyTo_1()
    {
        IList pair = new Pair<int, string>();
        var array = new object[2];
        pair.CopyTo (array, 0);
    }

    [TestMethod]
    public void Pair_Count_1()
    {
        IList pair = new Pair<int, string>();
        Assert.AreEqual (2, pair.Count);
    }

    [TestMethod]
    public void Pair_SyncRoot_1()
    {
        IList pair = new Pair<int, string>();
        Assert.IsNotNull (pair.SyncRoot);
    }

    [TestMethod]
    public void Pair_IsSynchronized_1()
    {
        IList pair = new Pair<int, string>();
        Assert.AreEqual (false, pair.IsSynchronized);
    }

    [TestMethod]
    public void Pair_GetEnumerator_1()
    {
        IList pair = new Pair<int, string> (1, "Hello");
        var array = new object[2];
        IEnumerator enumerator = pair.GetEnumerator();
        enumerator.MoveNext();
        array[0] = enumerator.Current!;
        enumerator.MoveNext();
        array[1] = enumerator.Current!;
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual ("Hello", array[1]);
    }

    [TestMethod]
    public void Pair_Clone_1()
    {
        var first = new Pair<int, string> (1, "Hello");
        var second = (Pair<int, string>)first.Clone();
        Assert.AreEqual (first.First, second.First);
        Assert.AreEqual (first.Second, second.Second);
    }

    [TestMethod]
    public void Pair_ReadOnly_1()
    {
        var pair = new Pair<int, string>();
        Assert.AreEqual (false, pair.ReadOnly);

        pair = new Pair<int, string> (1, "Hello", true);
        Assert.AreEqual (true, pair.ReadOnly);
    }

    [TestMethod]
    public void Pair_AsReadOnly_1()
    {
        var first = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (false, first.ReadOnly);

        var second = first.AsReadOnly();
        Assert.AreEqual (first.First, second.First);
        Assert.AreEqual (first.Second, second.Second);
        Assert.AreEqual (true, second.ReadOnly);
    }

    [TestMethod]
    [ExpectedException (typeof (ReadOnlyException))]
    public void Pair_ThrowIfReadOnly_1()
    {
        var pair = new Pair<int, string> (1, "Hello", true);
        pair.ThrowIfReadOnly();
    }

    [TestMethod]
    public void Pair_Equals_1()
    {
        var first = new Pair<int, string> (1, "Hello");
        var second = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (true, first.Equals (second));

        second = new Pair<int, string> (2, "World");
        Assert.AreEqual (false, first.Equals (second));
    }

    [TestMethod]
    public void Pair_Equals_2()
    {
        var first = new Pair<int, string> (1, "Hello");
        object second = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (true, first.Equals (second));

        second = new Pair<int, string> (2, "World");
        Assert.AreEqual (false, first.Equals (second));

        Assert.AreEqual (false, first.Equals (null));
        Assert.AreEqual (true, first!.Equals ((object)first));
        Assert.AreEqual (false, first.Equals ("Hello"));
    }

    [TestMethod]
    public void Pair_GetHashCode_1()
    {
        var first = new Pair<int, string> (1, "Hello");
        var second = new Pair<int, string> (2, "World");
        Assert.AreNotEqual
            (
                first.GetHashCode(),
                second.GetHashCode()
            );
    }

    [TestMethod]
    public void Pair_ToString_1()
    {
        var pair = new Pair<int, string> (1, "Hello");
        Assert.AreEqual ("1;Hello", pair.ToString());
    }

    [TestMethod]
    public void Pair_SetReadOnly_1()
    {
        var pair = new Pair<int, string> (1, "Hello");
        Assert.AreEqual (false, pair.ReadOnly);
        pair.SetReadOnly();
        Assert.AreEqual (true, pair.ReadOnly);
    }
}
