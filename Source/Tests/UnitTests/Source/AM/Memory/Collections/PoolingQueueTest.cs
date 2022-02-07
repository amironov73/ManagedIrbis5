// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Memory.Collections.Specialized;

#nullable enable

namespace UnitTests.AM.Memory.Collections.Specialized;

[TestClass]
public class PoolingQueueTests
{
    [TestMethod]
    public void TestEmpty()
    {
        using var list = new PoolingQueueRef<object>();

        Assert.AreEqual (0, list.Count);
        Assert.AreEqual (true, list.IsEmpty);
    }

    [TestMethod]
    public void SingleAddition()
    {
        using var list = new PoolingQueueRef<object>();
        list.Enqueue (30);

        Assert.AreEqual (1, list.Count);
        Assert.AreEqual (30, list.Dequeue());
    }

    [TestMethod]
    public void AllAdditions()
    {
        using var list = new PoolingQueueRef<object>();

        for (int i = 0; i < PoolsDefaults.DefaultPoolBucketSize; i++)
        {
            list.Enqueue (i * 10);
        }

        Assert.AreEqual (PoolsDefaults.DefaultPoolBucketSize, list.Count);
    }

    [TestMethod]
    public void SingleAdditionAndRemove()
    {
        using var list = new PoolingQueueRef<object>();
        list.Enqueue (30);
        list.Enqueue (60);
        list.Dequeue();

        Assert.AreEqual (1, list.Count);
        Assert.AreEqual (60, list.Dequeue());
    }
}
