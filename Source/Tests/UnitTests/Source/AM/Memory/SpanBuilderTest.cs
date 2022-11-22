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
public sealed class SpanBuilderTest
{
    [TestMethod]
    [Description ("С оператором new")]
    public void SpanBuilder_Build_1()
    {
        var memory = new char[100];
        var builder = new SpanBuilder<char> (memory);
        builder.Append ("Hello");
        builder.Append (", ");
        builder.Append ("world");
        var result = builder.Build().ToString();

        Assert.AreEqual ("Hello, world", result);
    }

    [TestMethod]
    [Description ("Со stackalloc")]
    public void SpanBuilder_Build_2()
    {
        Span<char> memory = stackalloc char[100];
        var builder = new SpanBuilder<char> (memory);
        builder.Append ("Hello");
        builder.Append (", ");
        builder.Append ("world");
        var result = builder.Build().ToString();

        Assert.AreEqual ("Hello, world", result);
    }
}
