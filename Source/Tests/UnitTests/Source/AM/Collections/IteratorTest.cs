// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.Collections;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class IteratorTest
{
    private class NIterator : Iterator<int>
    {
        private readonly int _limit;

        public NIterator (int limit) => _limit = limit;

        public override bool MoveNext() => (_current = ++State) < _limit;
    }

    [TestMethod]
    public void Iterator_Construction_1()
    {
        var iterator = new NIterator (0);
        var array = iterator.ToArray();

        Assert.AreEqual (0, array.Length);
    }

    [TestMethod]
    public void Iterator_Construction_2()
    {
        var iterator = new NIterator (10);
        var array = iterator.ToArray();

        Assert.AreEqual (10, array.Length);
        Assert.AreEqual (0, array[0]);
        Assert.AreEqual (1, array[1]);
        Assert.AreEqual (9, array[9]);
    }

    [TestMethod]
    public void Iterator_Construction_3()
    {
        var enumerable = (IEnumerable)new NIterator (0);
        var enumerator = enumerable.GetEnumerator();
        Assert.IsFalse (enumerator.MoveNext());
    }

    [TestMethod]
    public void Iterator_Construction_4()
    {
        var enumerable = (IEnumerable)new NIterator (2);
        var enumerator = enumerable.GetEnumerator();
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (0, enumerator.Current);
        Assert.IsTrue (enumerator.MoveNext());
        Assert.AreEqual (1, enumerator.Current);
        Assert.IsFalse (enumerator.MoveNext());
        Assert.AreEqual (2, enumerator.Current);
    }
}
