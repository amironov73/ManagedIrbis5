// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Memory;

#endregion

#nullable enable

namespace UnitTests.AM.Memory;

[TestClass]
public sealed class UnsafeBuilderTest
{
    [TestMethod]
    [Description ("Форматирование чисел")]
    public unsafe void SpanBuilder_AppendInvariant_1()
    {
        const int length = 100;
        var array = new char[length];
        fixed (char* pointer = array)
        {
            var builder = new UnsafeBuilder<char> (pointer, length);
            builder.Append ("Hello");
            builder.AppendInvariant (10);
            builder.Append ("times");
            var result = builder.Build().ToString();

            Assert.AreEqual ("Hello10times", result);
        }
    }

    [TestMethod]
    [Description ("Пустой результат")]
    public unsafe void SpanBuilder_Build_1()
    {
        const int length = 100;
        var array = new char[length];
        fixed (char* pointer = array)
        {
            var builder = new UnsafeBuilder<char> (pointer, length);
            var result = builder.Build().ToString();

            Assert.AreEqual (string.Empty, result);
        }
    }

    [TestMethod]
    [Description ("Hello, world")]
    public unsafe void SpanBuilder_Build_2()
    {
        const int length = 100;
        var array = new char[length];
        fixed (char* pointer = array)
        {
            var builder = new UnsafeBuilder<char> (pointer, length);
            builder.Append ("Hello");
            builder.Append (", ");
            builder.Append ("world");
            var result = builder.Build().ToString();

            Assert.AreEqual ("Hello, world", result);
        }
    }

    [TestMethod]
    [Description ("Добавление по одному элементу")]
    public unsafe void SpanBuilder_Build_3()
    {
        const int length = 100;
        var array = new char[length];
        fixed (char* pointer = array)
        {
            var builder = new UnsafeBuilder<char> (pointer, length);
            builder.Append ('H');
            builder.Append ('e');
            builder.Append ('l');
            builder.Append ('l');
            builder.Append ('o');
            var result = builder.Build().ToString();

            Assert.AreEqual ("Hello", result);
        }
    }
}
