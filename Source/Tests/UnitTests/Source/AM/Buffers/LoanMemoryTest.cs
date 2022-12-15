// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using System.Buffers;
using System.Globalization;

using AM.Buffers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.Buffers;

[TestClass]
public sealed class LoanMemoryTest
{
    [TestMethod]
    [Description ("Занимаем память из общего пула")]
    public void LoanMemory_Construction_1()
    {
        const int minLength = 100;
        using var memory = new LoanMemory<byte> (minLength);

        Assert.IsNotNull (memory.Pool);
        Assert.IsNotNull (memory.Array);
        Assert.IsTrue (memory.Array.Length >= minLength);
        Assert.AreEqual (minLength, memory.Length);
        Assert.AreEqual (minLength, memory.Length);
        Assert.AreEqual (minLength, memory.AsMemory().Length);
        Assert.AreEqual (minLength, memory.AsSpan().Length);
    }

    [TestMethod]
    [Description ("Занимаем память из конкретного пула")]
    public void LoanMemory_Construction_2()
    {
        const int minLength = 100;
        var pool = ArrayPool<byte>.Create();
        using var memory = new LoanMemory<byte> (minLength, pool);

        Assert.AreSame (pool, memory.Pool);
        Assert.IsNotNull (memory.Array);
        Assert.IsTrue (memory.Array.Length >= minLength);
        Assert.AreEqual (minLength, memory.Length);
        Assert.AreEqual (minLength, memory.Length);
        Assert.AreEqual (minLength, memory.AsMemory().Length);
        Assert.AreEqual (minLength, memory.AsSpan().Length);
    }

    [TestMethod]
    [Description ("Память под текст")]
    public void LoanMemory_ForText_1()
    {
        const string helloWorld = "Hello, World!";
        var minLength = helloWorld.Length; // работает только для UTF-8 и Latin1!
        using var memory = LoanMemory<byte>.ForText (helloWorld);

        Assert.IsNotNull (memory.Pool);
        Assert.IsNotNull (memory.Array);
        Assert.IsTrue (memory.Array.Length >= minLength);
        Assert.AreEqual (minLength, memory.Length);
        Assert.AreEqual (minLength, memory.Length);
        Assert.AreEqual (minLength, memory.AsMemory().Length);
        Assert.AreEqual (minLength, memory.AsSpan().Length);

        Assert.AreEqual ((byte) 'H', memory.Array[0]);
        Assert.AreEqual ((byte) 'e', memory.Array[1]);
        Assert.AreEqual ((byte) 'l', memory.Array[2]);
        Assert.AreEqual ((byte) 'l', memory.Array[3]);
        Assert.AreEqual ((byte) 'o', memory.Array[4]);
        Assert.AreEqual ((byte) ',', memory.Array[5]);
        Assert.AreEqual ((byte) ' ', memory.Array[6]);
        Assert.AreEqual ((byte) 'W', memory.Array[7]);
        Assert.AreEqual ((byte) 'o', memory.Array[8]);
        Assert.AreEqual ((byte) 'r', memory.Array[9]);
        Assert.AreEqual ((byte) 'l', memory.Array[10]);
        Assert.AreEqual ((byte) 'd', memory.Array[11]);
        Assert.AreEqual ((byte) '!', memory.Array[12]);
    }

    [TestMethod]
    [Description ("Память под расформатирование числа")]
    public void LoanMemory_ForNumber_1()
    {
        const double pi = 3.1415926;
        using var memory = LoanMemory<char>.ForNumber (pi, provider: CultureInfo.InvariantCulture);

        Assert.IsNotNull (memory.Pool);
        Assert.IsNotNull (memory.Array);
        Assert.AreEqual (9, memory.Length);
        Assert.AreEqual ('3', memory.Array[0]);
        Assert.AreEqual ('.', memory.Array[1]);
        Assert.AreEqual ('1', memory.Array[2]);
        Assert.AreEqual ('4', memory.Array[3]);
        Assert.AreEqual ('1', memory.Array[4]);
        Assert.AreEqual ('5', memory.Array[5]);
        Assert.AreEqual ('9', memory.Array[6]);
        Assert.AreEqual ('2', memory.Array[7]);
        Assert.AreEqual ('6', memory.Array[8]);
    }
}
