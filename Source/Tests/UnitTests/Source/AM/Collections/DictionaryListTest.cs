// ReSharper disable CheckNamespace

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public class DictionaryListTest
{
    [TestMethod]
    public void DictionaryList_Constructor()
    {
        var list
            = new DictionaryList<string, int>();

        Assert.IsNotNull (list);
        Assert.AreEqual (0, list.Count);

        var items = list.ToArray();
        Assert.AreEqual (0, items.Length);
    }

    [TestMethod]
    public void DictionaryList_Add()
    {
        var list
            = new DictionaryList<string, int>();
        Assert.AreEqual (0, list.Count);

        list.Add ("one", 1);
        Assert.AreEqual (1, list.Count);

        list.Add ("two", 2);
        Assert.AreEqual (2, list.Count);

        list.Add ("one", 11);
        Assert.AreEqual (2, list.Count);

        list.Clear();
        Assert.AreEqual (0, list.Count);
    }

    [TestMethod]
    public void DictionaryList_AddRange()
    {
        var list
            = new DictionaryList<string, int>();

        list.AddRange ("one", new[] { 1, 11 });
        list.AddRange ("two", new[] { 2 });

        Assert.AreEqual (2, list.Count);
        Assert.AreEqual (2, list["one"].Length);
        Assert.AreEqual (1, list["two"].Length);
        Assert.AreEqual (0, list["three"].Length);
    }

    [TestMethod]
    public void DictionaryList_Clear()
    {
        var list
            = new DictionaryList<string, int>();

        list.AddRange ("one", new[] { 1, 11 });
        list.AddRange ("two", new[] { 2 });
        list.Clear();

        Assert.AreEqual (0, list.Count);
    }

    [TestMethod]
    public void DictionaryList_Keys()
    {
        var list
            = new DictionaryList<string, int>();
        Assert.AreEqual (0, list.Keys.Length);

        list.Add ("one", 1);
        list.Add ("two", 2);
        list.Add ("one", 11);

        var keys = list.Keys;
        Assert.AreEqual (2, keys.Length);
    }

    [TestMethod]
    public void DictionaryList_Values1()
    {
        var list
            = new DictionaryList<string, int>();
        Assert.AreEqual (0, list.Keys.Length);

        list.Add ("one", 1);
        list.Add ("two", 2);
        list.Add ("one", 11);

        var values = list["one"];
        Assert.AreEqual (2, values.Length);

        values = list["two"];
        Assert.AreEqual (1, values.Length);

        values = list["three"];
        Assert.AreEqual (0, values.Length);
    }

    [TestMethod]
    public void DictionaryList_Values2()
    {
        var list
            = new DictionaryList<string, int>();
        Assert.AreEqual (0, list.Keys.Length);

        list.Add ("one", 1);
        list.Add ("two", 2);
        list.Add ("one", 11);

        var values = list.GetValues ("one");
        Assert.IsNotNull (values);
        Assert.AreEqual (2, values!.Count);

        values = list.GetValues ("two");
        Assert.IsNotNull (values);
        Assert.AreEqual (1, values!.Count);

        values = list.GetValues ("three");
        Assert.IsNull (values);
    }

    [TestMethod]
    public void DictionaryList_Enumeration()
    {
        var list
            = new DictionaryList<string, int>();
        Assert.AreEqual (0, list.Keys.Length);

        list.Add ("one", 1);
        list.Add ("two", 2);
        list.Add ("one", 11);

        var pairs = list.ToArray();
        Assert.IsNotNull (pairs);
        Assert.AreEqual (2, pairs.Length);
    }
}
