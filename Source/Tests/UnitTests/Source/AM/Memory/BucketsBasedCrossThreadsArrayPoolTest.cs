// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using AM.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTests.AM.Memory;

[TestClass]
public sealed class BucketsBasedCrossThreadsArrayPoolTest
{
    [TestMethod]
    public void BucketsBasedCrossThreadsArrayPool_Construction_1()
    {
        var shared = BucketsBasedCrossThreadsArrayPool<byte>.Shared;
        Assert.IsNotNull (shared);
    }

    [TestMethod]
    public void BucketsBasedCrossThreadsArrayPool_Rent_1()
    {
        const int minimum = 1024;
        var instance = new BucketsBasedCrossThreadsArrayPool<byte>();
        var array = instance.Rent (minimum);
        Assert.IsNotNull (array);
        Assert.IsTrue (array.Length >= minimum);
        instance.Return (array);
    }

    [TestMethod]
    public void BucketsBasedCrossThreadsArrayPool_Rent_2()
    {
        const int minimum = 1024;
        var array = BucketsBasedCrossThreadsArrayPool<byte>.Shared.Rent (minimum);
        Assert.IsNotNull (array);
        Assert.IsTrue (array.Length >= minimum);
        BucketsBasedCrossThreadsArrayPool<byte>.Shared.Return (array);
    }
}
