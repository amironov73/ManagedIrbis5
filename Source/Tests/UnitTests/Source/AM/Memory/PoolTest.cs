// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Memory;

#endregion

#nullable enable

namespace UnitTests.AM.Memory;

[TestClass]
public sealed class PoolTest
{
    private sealed class Dummy
    {
        public int Value { get; init; }
    }

    [TestMethod]
    [Description ("Получение объекта из пула с возвратом")]
    public void PoolT_Get_1()
    {
        var dummy = Pool<Dummy>.Get();
        Assert.IsNotNull (dummy);
        var value = dummy.Value;

        Pool<Dummy>.Return (dummy);
        Assert.AreEqual (value, dummy.Value);
    }

    [TestMethod]
    [Description ("Получение непрерывного блока памяти")]
    public void Pool_GetBuffer_1()
    {
        const int expected = 16;
        using var owner = Pool.GetBuffer<Dummy> (expected);
        Assert.IsNotNull (owner);
        Assert.AreEqual (expected, owner.Length());

        var span = owner.Memory.Span;
        span[0] = new Dummy { Value = expected };
        Assert.AreEqual (expected, span[0].Value);
    }

    [TestMethod]
    [Description ("Получение непрерывного блока памяти")]
    public void Pool_GetBufferFrom_1()
    {
        const int expected = 16;
        Span<Dummy> source = new Dummy[expected];
        using var owner = Pool.GetBufferFrom<Dummy> (source);
        Assert.IsNotNull (owner);
        Assert.AreEqual (expected, owner.Length());

        var span = owner.Memory.Span;
        span[0] = new Dummy { Value = expected };
        Assert.AreEqual (expected, span[0].Value);
    }

    [TestMethod]
    [Description ("Получение объекта из пула с возвратом")]
    public void Pool_Get_1()
    {
        var dummy = Pool.Get<Dummy>();
        Assert.IsNotNull (dummy);
        var value = dummy.Value;

        Pool.Return (dummy);
        Assert.AreEqual (value, dummy.Value);
    }
}
