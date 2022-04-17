using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach

#nullable enable

namespace UnitTests.AM;

[TestClass]
public sealed class ChainMemoryTest
{
    private static readonly int[] _array1 = { 1, 2, 3, 4 };
    private static readonly int[] _array2 = { 5, 6, 7 };
    private static readonly int[] _array3 = { 8, 9 };

    [TestMethod]
    public void ChainMemory_Constructor_1()
    {
        var head = new ChainMemory<int> (_array1);
        var tail = head.Append (_array2);
        tail.Append (_array3);

        Assert.AreEqual (9, head.TotalLength());

        var array = head.ToArray();
        Assert.AreEqual (9, array.Length);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (2, array[1]);
        Assert.AreEqual (9, array[8]);
    }

    [TestMethod]
    public void ChainMemory_Enumerator_1()
    {
        var head = new ChainMemory<int> (_array1);
        var tail = head.Append (_array2);
        tail.Append (_array3);

        int index = 0;
        foreach (var item in head)
        {
            Assert.AreEqual (++index, item);
        }

        Assert.AreEqual (9, index);
    }
}
