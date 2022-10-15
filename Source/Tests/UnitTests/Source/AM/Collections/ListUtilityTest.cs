﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public class ListUtilityTest
{
    [TestMethod]
    public void ListUtility_IsNullOrEmpty_1()
    {
        List<int>? list = null;
        Assert.IsTrue (list.IsNullOrEmpty());

        list = new List<int>();
        Assert.IsTrue (list.IsNullOrEmpty());

        list.Add (1);
        Assert.IsFalse (list.IsNullOrEmpty());
    }

    [TestMethod]
    public void ListUtility_ContainsValue_1()
    {
        var list = new List<int> { 1, 2, 3 };

        Assert.IsTrue (list.ContainsValue
            (
                1,
                EqualityComparer<int>.Default
            ));

        Assert.IsFalse (list.ContainsValue
            (
                4,
                EqualityComparer<int>.Default
            ));
    }

    [TestMethod]
    public void ListUtility_AddDistinct_1()
    {
        var list = new List<int> { 1, 2, 3 };

        Assert.IsTrue (list.AddDistinct (4));
        Assert.IsFalse (list.AddDistinct (4));

        list = new List<int> { 1, 2, 3 };
        IEqualityComparer<int> comparer
            = EqualityComparer<int>.Default;

        Assert.IsTrue (list.AddDistinct (4, comparer));
        Assert.IsFalse (list.AddDistinct (4, comparer));
    }

    [TestMethod]
    public void ListUtility_AddRangeDistinct_1()
    {
        var list = new List<int> { 1, 2, 3 };

        IEqualityComparer<int> comparer
            = EqualityComparer<int>.Default;

        Assert.IsTrue (list.AddRangeDistinct (new[] { 4, 5, 6 }, comparer));
        Assert.IsFalse (list.AddRangeDistinct (new[] { 6, 7, 8 }, comparer));
    }

    [TestMethod]
    public void ListUtility_IndexOf_1()
    {
        var list = new List<int> { 1, 2, 3 };

        Func<int, bool> predicate = i => i == 2;
        Assert.AreEqual (1, list.IndexOf (predicate));

        predicate = i => i == 4;
        Assert.IsTrue (list.IndexOf (predicate) < 0);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentException))]
    public void ListUtility_ThrowIfNullOrEmpty_1()
    {
        var list = new List<int>();
        ListUtility.ThrowIfNullOrEmpty (list);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentException))]
    public void ListUtility_ThrowIfNullOrEmpty_2()
    {
        var list = new List<int>();
        ListUtility.ThrowIfNullOrEmpty (list, nameof (list));
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentException))]
    public void ListUtility_ThrowIfNullOrEmpty_3()
    {
        var array = Array.Empty<int>();
        ListUtility.ThrowIfNullOrEmpty (array);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentException))]
    public void ListUtility_ThrowIfNullOrEmpty_4()
    {
        var array = Array.Empty<int>();
        ListUtility.ThrowIfNullOrEmpty (array);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentNullException))]
    public void ListUtility_ThrowIfNullOrEmpty_5()
    {
        List<int>? list = null;
        ListUtility.ThrowIfNullOrEmpty (list);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentNullException))]
    public void ListUtility_ThrowIfNullOrEmpty_6()
    {
        List<int>? list = null;
        ListUtility.ThrowIfNullOrEmpty (list);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentNullException))]
    public void ListUtility_ThrowIfNullOrEmpty_7()
    {
        int[]? array = null;
        ListUtility.ThrowIfNullOrEmpty (array);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentNullException))]
    public void ListUtility_ThrowIfNullOrEmpty_8()
    {
        int[]? array = null;
        ListUtility.ThrowIfNullOrEmpty (array);
    }

    [TestMethod]
    public void ListUtility_ThrowIfNullOrEmpty_9()
    {
        var first = new List<int> { 1, 2, 3 };
        var second = (List<int>)ListUtility.ThrowIfNullOrEmpty (first);
        Assert.AreSame (first, second);
    }

    [TestMethod]
    public void ListUtility_ThrowIfNullOrEmpty_10()
    {
        var first = new List<int> { 1, 2, 3 };
        var second = (List<int>) ListUtility.ThrowIfNullOrEmpty (first);
        Assert.AreSame (first, second);
    }

    [TestMethod]
    public void ListUtility_ThrowIfNullOrEmpty_11()
    {
        int[] first = { 1, 2, 3 };
        var second = ListUtility.ThrowIfNullOrEmpty (first);
        Assert.AreSame (first, second);
    }

    [TestMethod]
    public void ListUtility_ThrowIfNullOrEmpty_12()
    {
        int[] first = { 1, 2, 3 };
        var second = ListUtility.ThrowIfNullOrEmpty (first);
        Assert.AreSame (first, second);
    }
}
