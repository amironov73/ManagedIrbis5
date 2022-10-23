// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using AM.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTests.AM.Memory;

[TestClass]
public sealed class BucketsBasedCrossThreadsMemoryPoolTest
{
    [TestMethod]
    public void BucketsBasedCrossThreadsMemoryPool_Construction_1()
    {
        var shared = BucketsBasedCrossThreadsMemoryPool<byte>.Shared;
        Assert.IsNotNull (shared);
    }

    [TestMethod]
    public void BucketsBasedCrossThreadsMemoryPool_Rent_1()
    {
        const int minimum = 1024;
        var instance = new BucketsBasedCrossThreadsMemoryPool<byte>();
        using var memory = instance.Rent (minimum);
        Assert.IsNotNull (memory);
        Assert.IsTrue (memory.Memory.Length >= minimum);
    }

    [TestMethod]
    public void BucketsBasedCrossThreadsMemoryPool_Rent_2()
    {
        const int minimum = 1024;
        using var memory = BucketsBasedCrossThreadsMemoryPool<byte>.Shared.Rent (minimum);
        Assert.IsNotNull (memory);
        Assert.IsTrue (memory.Memory.Length >= minimum);
    }
}
