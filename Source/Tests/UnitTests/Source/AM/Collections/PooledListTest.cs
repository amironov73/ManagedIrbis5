// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class PooledListTest
{
    private static T[] Get<T> (ReadOnlySpan<char> span)
    {
        var result = new T[span.Length];
        for (var i = 0; i < span.Length; i++)
        {
            result[i] = (T)Convert.ChangeType (span[i], typeof (T));
        }

        return result;
    }

    private static bool Same<T>
        (
            Span<T> one,
            Span<T> two
        )
        where T : IEquatable<T>
    {
        if (one.Length != two.Length)
        {
            return false;
        }

        for (var i = 0; i < one.Length; i++)
        {
            if (!one[i].Equals (two[i]))
            {
                return false;
            }
        }

        return true;
    }

    [TestMethod]
    public void PooledList_Constructor_1()
    {
        using var list = new PooledList<int>();

        Assert.AreEqual (0, list.Length);
        Assert.AreEqual (0, list.Capacity);
        Assert.AreEqual (0, list.ToArray().Length);
    }

    [TestMethod]
    public void PooledList_Append_1()
    {
        using var list = new PooledList<int>();

        list.Append ('H');
        Assert.AreEqual (1, list.Length);
        list.Append ('e');
        Assert.AreEqual (2, list.Length);
        list.Append ('l');
        Assert.AreEqual (3, list.Length);
        list.Append ('l');
        Assert.AreEqual (4, list.Length);
        list.Append ('o');
        Assert.AreEqual (5, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> ("Hello"),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_2()
    {
        using var list = new PooledList<int>();

        list.Append (Get<int> ("Hello, "));
        Assert.AreEqual (7, list.Length);
        list.Append (Get<int> ("world!"));
        Assert.AreEqual (13, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> ("Hello, world!"),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_3()
    {
        using var list = new PooledList<int>();

        list.Append (Get<int> ("Hello, "), Get<int> ("world!"));
        Assert.AreEqual (13, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> ("Hello, world!"),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_4()
    {
        using var list = new PooledList<int>();
        var longString = new string ('x', 80);

        list.Append (Get<int> (longString));
        Assert.AreEqual (longString.Length, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> (longString),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_5()
    {
        using var list = new PooledList<int>();
        var longString = new string ('x', 80);

        list.Append (Get<int> (longString), Get<int> (longString));
        Assert.AreEqual (longString.Length * 2, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> (longString + longString),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_6()
    {
        using var list = new PooledList<int>();
        var longString = new string ('x', 80);

        list.Append (Get<int> (longString));
        list.Append (Get<int> (longString));
        Assert.AreEqual (longString.Length * 2, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> (longString + longString),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_7()
    {
        using var list = new PooledList<int>();
        var longString = new string ('x', 80);

        list.Append (Get<int> (longString), Get<int> (longString),
            Get<int> (longString));
        Assert.AreEqual (longString.Length * 3, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> (longString + longString + longString),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_Append_8()
    {
        using var list = new PooledList<int>();

        for (var i = 0; i < 100; i++)
        {
            list.Append ('x');
        }

        Assert.AreEqual (100, list.Length);
        Assert.IsTrue (Same<int>
            (
                Get<int> (new string ('x', 100)),
                list.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_EnsureCapacity_1()
    {
        using var list = new PooledList<int>();

        list.EnsureCapacity (100);
        Assert.IsTrue (100 <= list.Capacity);
    }

    [TestMethod]
    public void PooledList_AsSpan_1()
    {
        using var list = new PooledList<int>();

        list.Append (Get<int> ("Hello, world"));
        var span = list.AsSpan (7);
        Assert.IsTrue (Same<int>
            (
                Get<int> ("world"),
                span.ToArray()
            ));
    }

    [TestMethod]
    public void PooledList_AsSpan_2()
    {
        using var list = new PooledList<int>();

        list.Append (Get<int> ("Hello, world"));
        var span = list.AsSpan (0, 5);
        Assert.IsTrue (Same<int>
            (
                Get<int> ("Hello"),
                span.ToArray()
            ));
    }
}
