// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class ListPoolTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ListPool_Construction_1()
    {
        using var pool = new ListPool<int>();
        Assert.AreEqual (0, pool.Count);
        Assert.IsFalse (pool.IsReadOnly);
        Assert.IsTrue (pool.Capacity > 0);
    }

    [TestMethod]
    [Description ("Конструктор с явно заданной емкостью")]
    public void ListPool_Construction_2()
    {
        const int capacity = 100;
        using var pool = new ListPool<int> (capacity);
        Assert.AreEqual (0, pool.Count);
        Assert.IsFalse (pool.IsReadOnly);
        Assert.IsTrue (pool.Capacity >= capacity);
    }

    [TestMethod]
    [Description ("Конструктор с начальным наполнением списка")]
    public void ListPool_Construction_3()
    {
        var initialData = new List<int>() { 1, 2, 3, 4 };
        using var pool = new ListPool<int> (initialData);
        Assert.AreEqual (initialData.Count, pool.Count);
        Assert.IsTrue (pool.Capacity >= initialData.Count);
    }

    [TestMethod]
    [Description ("Конструктор с начальным наполнением списка")]
    public void ListPool_Construction_4()
    {
        var initialData = new [] { 1, 2, 3, 4 };
        using var pool = new ListPool<int> (initialData);
        Assert.AreEqual (initialData.Length, pool.Count);
        Assert.IsTrue (pool.Capacity >= initialData.Length);
    }

    [TestMethod]
    [Description ("Конструктор с начальным наполнением списка")]
    public void ListPool_Construction_5()
    {
        var initialData = new [] { 1, 2, 3, 4 };
        using var pool = new ListPool<int> (initialData.AsSpan());
        Assert.AreEqual (initialData.Length, pool.Count);
        Assert.IsTrue (pool.Capacity >= initialData.Length);
    }

    [TestMethod]
    [Description ("Добавление элементов без необходимости увеличения емкости")]
    public void ListPool_Add_1()
    {
        using var pool = new ListPool<int>
        {
            1, 2, 3, 4, 5
        };
        Assert.AreEqual (5, pool.Count);
        Assert.IsTrue (pool.Capacity >= 5);
    }

    [TestMethod]
    [Description ("Добавление элементов с необходимостью увеличения емкости")]
    public void ListPool_Add_2()
    {
        const int count = 200;
        using var pool = new ListPool<int>();
        for (var i = 0; i < count; i++)
        {
            pool.Add (i);
        }
        Assert.AreEqual (count, pool.Count);
        Assert.IsTrue (pool.Capacity >= count);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void ListPool_Clear_1()
    {
        var initialData = new [] { 1, 2, 3, 4 };
        using var pool = new ListPool<int> (initialData);
        pool.Clear();
        Assert.AreEqual (0, pool.Count);
    }

    [TestMethod]
    [Description ("Проверка присутствия элемента в списке")]
    public void ListPool_Contains_1()
    {
        var initialData = new [] { 1, 2, 3, 4 };
        using var pool = new ListPool<int> (initialData);
        foreach (var item in initialData)
        {
            Assert.IsTrue (pool.Contains (item));
        }

        Assert.IsFalse (pool.Contains (-1));
        Assert.IsFalse (pool.Contains (0));
    }

    [TestMethod]
    [Description ("Поиск элемента в списке")]
    public void ListPool_IndexOf_1()
    {
        var initialData = new [] { 1, 2, 3, 4 };
        using var pool = new ListPool<int> (initialData);
        for (var index = 0; index < initialData.Length; index++)
        {
            Assert.AreEqual (index, pool.IndexOf (initialData[index]));
        }

        Assert.IsTrue (pool.IndexOf (-1) < 0);
        Assert.IsTrue (pool.IndexOf (0) < 0);
    }
}
