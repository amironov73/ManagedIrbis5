// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
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
public sealed class DisposableCollectionTest
{
    private static int _count;

    private sealed class Dummy
        : IDisposable
    {
        public void Dispose()
        {
            _count++;
        }
    }

    [TestMethod]
    public void DisposableCollection_Dispose()
    {
        _count = 0;

        var collection = new DisposableCollection<Dummy>
        {
            new (),
            new (),
            new ()
        };

        collection.Dispose();

        Assert.AreEqual (3, _count);
    }
}
