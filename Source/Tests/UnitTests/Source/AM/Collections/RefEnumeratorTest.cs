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

using AM;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class SpanEnumeratorTest
{
    [TestMethod]
    public void RefEnumerator_GetEnumerator_1()
    {
        int[] array = { 1, 2, 3, 4, 5, 6 };

        foreach (ref var item in array.AsRefEnumerable())
        {
            item *= 2;
        }

        Assert.AreEqual (2, array[0]);
        Assert.AreEqual (4, array[1]);
        Assert.AreEqual (6, array[2]);
        Assert.AreEqual (8, array[3]);
        Assert.AreEqual (10, array[4]);
        Assert.AreEqual (12, array[5]);
    }

    [TestMethod]
    public void RefEnumerator_GetEnumerator_2()
    {
        var array = Array.Empty<int>();

        foreach (ref var item in array.AsRefEnumerable())
        {
            item *= 2;
        }

        Assert.AreEqual (0, array.Length);
    }
}
