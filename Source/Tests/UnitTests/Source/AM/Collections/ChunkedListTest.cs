// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.Collections;

using AM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class ChunkedListTest
{
    [TestMethod]
    public void ChunkedList_Construction_1()
    {
        var list = new ChunkedList<int>();
        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (0, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_Construction_2()
    {
        var list = new ChunkedList<int> (1);
        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (ChunkedList<int>.ChunkSize, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_Construction_3()
    {
        var chunkSize = ChunkedList<int>.ChunkSize;
        var list = new ChunkedList<int> (chunkSize);
        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (chunkSize, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_Construction_4()
    {
        var list = new ChunkedList<int> (1025);
        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (1536, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_Add_1()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        Assert.AreEqual (1, list.Count);
        Assert.AreEqual (ChunkedList<int>.ChunkSize, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_Add_2()
    {
        var chunkSize = ChunkedList<int>.ChunkSize;
        var list = new ChunkedList<int>();
        var desiredCount = chunkSize + 1;
        for (var i = 0; i < desiredCount; i++)
        {
            list.Add (i);
        }

        Assert.AreEqual (desiredCount, list.Count);
        Assert.AreEqual (chunkSize * 2, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_CopyTo_1()
    {
        var list = new ChunkedList<int>();
        var array = new int[3];
        list.CopyTo (array, 0);
        Assert.AreEqual (0, array[0]);
        Assert.AreEqual (0, array[1]);
        Assert.AreEqual (0, array[2]);
    }

    [TestMethod]
    public void ChunkedList_CopyTo_2()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        var array = new int[3];
        list.CopyTo (array, 0);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (0, array[1]);
        Assert.AreEqual (0, array[2]);
    }

    [TestMethod]
    public void ChunkedList_CopyTo_3()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        list.Add (2);
        list.Add (3);
        var array = new int[3];
        list.CopyTo (array, 0);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (2, array[1]);
        Assert.AreEqual (3, array[2]);
    }

    [TestMethod]
    public void ChunkedList_CopyTo_4()
    {
        var count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (i);
        }

        var array = new int[count * 2];
        list.CopyTo (array, 0);
        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual (i, array[i]);
            Assert.AreEqual (0, array[count + i]);
        }
    }

    [TestMethod]
    public void ChunkedList_Clear_1()
    {
        var chunkSize = ChunkedList<int>.ChunkSize;
        var list = new ChunkedList<int>();
        list.Add (1);
        list.Clear();
        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (chunkSize, list.Capacity);
    }

    [TestMethod]
    public void ChunkedList_Contains_1()
    {
        var list = new ChunkedList<int>();
        Assert.IsFalse (list.Contains (2));
        list.Add (1);
        Assert.IsFalse (list.Contains (2));
        list.Add (2);
        Assert.IsTrue (list.Contains (2));
        list.Add (2);
        Assert.IsTrue (list.Contains (2));
    }

    [TestMethod]
    public void ChunkedList_Contains_2()
    {
        var list = new ChunkedList<int>();
        Assert.IsFalse (list.Contains (2));
        for (var i = 0; i < 1025; i++)
        {
            list.Add (i);
        }

        Assert.IsTrue (list.Contains (2));
        Assert.IsTrue (list.Contains (1023));
        Assert.IsTrue (list.Contains (1024));
        Assert.IsFalse (list.Contains (1025));
    }

    [TestMethod]
    public void ChunkedList_IsReadOnly_1()
    {
        var list = new ChunkedList<int>();
        Assert.IsFalse (list.IsReadOnly);
    }

    [TestMethod]
    public void ChunkedList_IndexOf_1()
    {
        var list = new ChunkedList<int>();
        Assert.IsTrue (list.IndexOf (2) < 0);

        list.Add (1);
        Assert.IsTrue (list.IndexOf (2) < 0);

        list.Add (2);
        Assert.AreEqual (1, list.IndexOf (2));

        list.Add (2);
        Assert.AreEqual (1, list.IndexOf (2));
    }

    [TestMethod]
    public void ChunkedList_IndexOf_2()
    {
        var count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (i);
        }

        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual (i, list.IndexOf (i));
        }

        Assert.IsTrue (list.IndexOf (count + 1) < 0);
    }

    [TestMethod]
    public void ChunkedList_Indexer_1()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        list.Add (2);
        list.Add (3);
        Assert.AreEqual (1, list[0]);
        Assert.AreEqual (2, list[1]);
        Assert.AreEqual (3, list[2]);
    }

    [TestMethod]
    public void ChunkedList_Indexer_2()
    {
        var count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (i);
        }

        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual (i, list[i]);
        }
    }

    [TestMethod]
    public void ChunkedList_Indexer_3()
    {
        var list = new ChunkedList<int>();
        list.Add (0);
        list.Add (0);
        list.Add (0);
        list[0] = 1;
        list[1] = 2;
        list[2] = 3;
        Assert.AreEqual (1, list[0]);
        Assert.AreEqual (2, list[1]);
        Assert.AreEqual (3, list[2]);
    }

    [TestMethod]
    public void ChunkedList_Indexer_4()
    {
        var count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (0);
        }

        for (var i = 0; i < count; i++)
        {
            list[i] = i;
        }

        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual (i, list[i]);
        }
    }

    [TestMethod]
    [ExpectedException (typeof (IndexOutOfRangeException))]
    public void ChunkedList_Indexer_5()
    {
        var list = new ChunkedList<int>();
        Assert.AreEqual (0, list[0]);
    }

    [TestMethod]
    [ExpectedException (typeof (IndexOutOfRangeException))]
    public void ChunkedList_Indexer_6()
    {
        var list = new ChunkedList<int>();
        list[0] = 0;
    }

    [TestMethod]
    [ExpectedException (typeof (IndexOutOfRangeException))]
    public void ChunkedList_Indexer_7()
    {
        var list = new ChunkedList<int>();
        list.Add (0);
        list.Add (0);
        list.Add (0);
        Assert.AreEqual (0, list[1000]);
    }

    [TestMethod]
    [ExpectedException (typeof (IndexOutOfRangeException))]
    public void ChunkedList_Indexer_8()
    {
        var list = new ChunkedList<int>();
        list.Add (0);
        list.Add (0);
        list.Add (0);
        list[1000] = 0;
    }

    [TestMethod]
    public void ChunkedList_GetEnumerator_1()
    {
        var list = new ChunkedList<int>();
        var enumerator = list.GetEnumerator();
        Assert.IsFalse (enumerator.MoveNext());
        enumerator.Dispose();
    }

    [TestMethod]
    public void ChunkedList_GetEnumerator_2()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        list.Add (2);
        list.Add (3);
        var enumerator = list.GetEnumerator();
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (1, enumerator.Current);
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (2, enumerator.Current);
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (3, enumerator.Current);
        Assert.IsFalse (enumerator.MoveNext());
        enumerator.Dispose();
    }

    [TestMethod]
    public void ChunkedList_GetEnumerator_3()
    {
        var count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (i);
        }

        var enumerator = list.GetEnumerator();
        for (var i = 0; i < count; i++)
        {
            Assert.IsTrue (enumerator.MoveNext());
            Assert.AreEqual (i, enumerator.Current);
        }

        Assert.IsFalse (enumerator.MoveNext());
        enumerator.Dispose();
    }

    [TestMethod]
    public void ChunkedList_GetEnumerator_4()
    {
        IEnumerable list = new ChunkedList<int>();
        var enumerator = list.GetEnumerator();
        Assert.IsFalse (enumerator.MoveNext());
    }

    [TestMethod]
    public void ChunkedList_GetEnumerator_5()
    {
        IEnumerable list = new ChunkedList<int> { 1, 2, 3 };
        var enumerator = list.GetEnumerator();
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (1, enumerator.Current);
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (2, enumerator.Current);
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (3, enumerator.Current);
        Assert.IsFalse (enumerator.MoveNext());
    }

    [TestMethod]
    public void ChunkedList_RemoveAt_1()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        list.Add (2);
        list.Add (3);
        list.RemoveAt (1);
        Assert.AreEqual (2, list.Count);
        Assert.AreEqual (1, list[0]);
        Assert.AreEqual (3, list[1]);
    }

    [TestMethod]
    public void ChunkedList_RemoveAt_2()
    {
        var count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (i);
        }

        list.RemoveAt (100);
        Assert.AreEqual (count - 1, list.Count);
        for (var i = 0; i < 100; i++)
        {
            Assert.AreEqual (i, list[i]);
        }

        for (var i = 100; i < count - 1; i++)
        {
            Assert.AreEqual (i + 1, list[i]);
        }
    }

    [TestMethod]
    [Description ("Удаление элемента с указанным значением")]
    public void ChunkedList_Remove_1()
    {
        var list = new ChunkedList<int>();
        Assert.IsFalse (list.Remove (2));

        list.Add (1);
        list.Add (2);
        list.Add (3);
        Assert.IsTrue (list.Remove (2));
        Assert.AreEqual (2, list.Count);
        Assert.AreEqual (3, list[1]);
        Assert.IsFalse (list.Remove (2));
    }

    [TestMethod]
    [Description ("Вставка в начало")]
    public void ChunkedList_Insert_1()
    {
        var list = new ChunkedList<int>();
        list.Insert (0, 3);
        list.Insert (0, 2);
        list.Insert (0, 1);
        Assert.AreEqual (3, list.Count);
        Assert.AreEqual (1, list[0]);
        Assert.AreEqual (2, list[1]);
        Assert.AreEqual (3, list[2]);
    }

    [TestMethod]
    [Description ("Вставка в середину")]
    public void ChunkedList_Insert_2()
    {
        var list = new ChunkedList<int>();
        list.Add (1);
        list.Add (5);
        list.Insert (1, 4);
        list.Insert (1, 3);
        list.Insert (1, 2);
        Assert.AreEqual (5, list.Count);
        Assert.AreEqual (1, list[0]);
        Assert.AreEqual (2, list[1]);
        Assert.AreEqual (3, list[2]);
        Assert.AreEqual (4, list[3]);
        Assert.AreEqual (5, list[4]);
    }

    [TestMethod]
    [Description ("Преобразование в массив: без переполнения")]
    public void ChunkedList_ToArray_1()
    {
        var list = new ChunkedList<int>();
        var array = list.ToArray();
        Assert.AreEqual (0, array.Length);

        list.Add (1);
        array = list.ToArray();
        Assert.AreEqual (1, array.Length);
        Assert.AreEqual (1, array[0]);

        list.Add (2);
        array = list.ToArray();
        Assert.AreEqual (2, array.Length);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (2, array[1]);

        list.Add (3);
        array = list.ToArray();
        Assert.AreEqual (3, array.Length);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (2, array[1]);
        Assert.AreEqual (3, array[2]);
    }

    [TestMethod]
    [Description ("Преобразование в массив: с переполнением")]
    public void ChunkedList_ToArray_2()
    {
        const int count = 1025;
        var list = new ChunkedList<int>();
        for (var i = 0; i < count; i++)
        {
            list.Add (i);
        }

        var array = list.ToArray();
        Assert.AreEqual (count, array.Length);
        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual (i, array[i]);
        }
    }
}
