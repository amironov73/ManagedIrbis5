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
    [Description ("Форматирование чисел")]
    public void SpanBuilder_AppendInvariant_1()
    {
        var memory = new char[100];
        var builder = new SpanBuilder<char> (memory);
        builder.Append ("Hello");
        builder.AppendInvariant (10);
        builder.Append ("times");
        var result = builder.Build().ToString();

        Assert.AreEqual ("Hello10times", result);
    }

    [TestMethod]
    [Description ("Пустой результат")]
    public void SpanBuilder_Build_1()
    {
        var memory = new char[100];
        var builder = new SpanBuilder<char> (memory);
        var result = builder.Build().ToString();

        Assert.AreEqual (string.Empty, result);
    }

    [TestMethod]
    [Description ("С оператором new")]
    public void SpanBuilder_Build_2()
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
    public void SpanBuilder_Build_3()
    {
        Span<char> memory = stackalloc char[100];
        var builder = new SpanBuilder<char> (memory);
        builder.Append ("Hello");
        builder.Append (", ");
        builder.Append ("world");
        var result = builder.Build().ToString();

        Assert.AreEqual ("Hello, world", result);
    }

    [TestMethod]
    [Description ("Добавление по одному элементу")]
    public void SpanBuilder_Build_4()
    {
        Span<char> memory = stackalloc char[100];
        var builder = new SpanBuilder<char> (memory);
        builder.Append ('H');
        builder.Append ('e');
        builder.Append ('l');
        builder.Append ('l');
        builder.Append ('o');
        var result = builder.Build().ToString();

        Assert.AreEqual ("Hello", result);
    }
}
