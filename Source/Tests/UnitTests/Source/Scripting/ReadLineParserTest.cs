// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Pidgin;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class ReadLineParserTest
{
    [TestMethod]
    [Description ("Обычная строка: нет перевода строки")]
    public void ReadLineParser_Parse_1()
    {
        var parser = Resolve.ReadLine();
        var result = parser.Parse ("hello");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Обычная строка: перевод строки '\n'")]
    public void ReadLineParser_Parse_2()
    {
        var parser = Resolve.ReadLine();
        var result = parser.Parse ("hello\nworld");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Обычная строка: перевод строки '\r\n'")]
    public void ReadLineParser_Parse_3()
    {
        var parser = Resolve.ReadLine();
        var result = parser.Parse ("hello\r\nworld");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void ReadLineParser_Parse_4()
    {
        var parser = Resolve.ReadLine();
        var result = parser.Parse (string.Empty);
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    [Description ("Зажевывание перевода строки: не зажевывать")]
    public void ReadLineParser_Parse_5()
    {
        var parser = Resolve.ReadLine().Then (Resolve.Remainder);
        var result = parser.Parse ("hello\nworld");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("\nworld", result.Value);

        result = parser.Parse ("hello\r\nworld");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("\r\nworld", result.Value);
    }

    [TestMethod]
    [Description ("Зажевывание перевода строки: зажевывать")]
    public void ReadLineParser_Parse_6()
    {
        var parser = Resolve.ReadLine (true).Then (Resolve.Remainder);
        var result = parser.Parse ("hello\nworld");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("world", result.Value);

        result = parser.Parse ("hello\r\nworld");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("world", result.Value);
    }

    [TestMethod]
    [Description ("Зажевывание перевода строки: зажевывать")]
    public void ReadLineParser_Parse_7()
    {
        var parser = Resolve.ReadLine (true).Then (Resolve.ReadLine (true));
        var result = parser.Parse ("hello\nworld\n");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("world", result.Value);

        result = parser.Parse ("hello\r\nworld\r\n");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("world", result.Value);
    }
}
