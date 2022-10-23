// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using AM.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.Memory;

[TestClass]
public sealed class JetPoolTest
{
    private sealed class Dummy
    {
        public string? TextProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    [TestMethod]
    public void JetPoolTest_Get_1()
    {
        const string text = "text";
        const int integer = 123;
        var pool = new JetPool<Dummy>();
        var first = pool.Get();
        Assert.IsNotNull (first);

        first.TextProperty = text;
        first.IntegerProperty = integer;
        Assert.AreEqual (text, first.TextProperty);
        Assert.AreEqual (integer, first.IntegerProperty);
        pool.Return (first);

        var second = pool.Get();
        second.TextProperty = text;
        second.IntegerProperty = integer;
        Assert.AreEqual (text, second.TextProperty);
        Assert.AreEqual (integer, second.IntegerProperty);
        pool.Return (second);
    }
}
