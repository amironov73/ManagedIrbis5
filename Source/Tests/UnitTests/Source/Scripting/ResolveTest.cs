// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Pidgin;
using static Pidgin.Parser<char>;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class ResolveTest
{
    [TestMethod]
    [Description ("Знак минус")]
    public void Resolve_Minus_1()
    {
        Assert.AreEqual ('-', Resolve.Minus.ParseOrThrow ("-"));
        Assert.ThrowsException<ParseException> (() => Resolve.Minus.ParseOrThrow ("+"));
    }

    [TestMethod]
    [Description ("Арабские цифры")]
    public void Resolve_Arabic_1()
    {
        Assert.AreEqual ('0', Resolve.Arabic.ParseOrThrow ("0"));
        Assert.ThrowsException<ParseException> (() => Resolve.Arabic.ParseOrThrow ("-"));
    }

    [TestMethod]
    [Description ("Идентификатор")]
    public void Resolve_Identifier_1()
    {
        var parser = Resolve.Identifier.Before (End);

        Assert.AreEqual ("a", parser.ParseOrThrow ("a"));
        Assert.AreEqual ("a1", parser.ParseOrThrow ("a1"));
        Assert.AreEqual ("_a1", parser.ParseOrThrow ("_a1"));
        Assert.AreEqual ("_", parser.ParseOrThrow ("_"));
        Assert.AreEqual ("a_1", parser.ParseOrThrow ("a_1"));
        Assert.AreEqual ("_1", parser.ParseOrThrow ("_1"));
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: пустая строка")]
    public void Resolve_EscapedLiteral_1()
    {
        var actual = Resolve.EscapedLiteral().ParseOrThrow ("\"\"");
        Assert.AreEqual (string.Empty, actual);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: пустой входной поток")]
    public void Resolve_EscapedLiteral_2()
    {
        var actual = Resolve.EscapedLiteral().Parse (string.Empty);
        Assert.IsFalse (actual.Success);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: неоткрытая строка")]
    public void Resolve_EscapedLiteral_3()
    {
        var actual = Resolve.EscapedLiteral().Parse ("hello");
        Assert.IsFalse (actual.Success);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: незакрытая строка")]
    public void Resolve_EscapedLiteral_4()
    {
        var actual = Resolve.EscapedLiteral().Parse ("\"hello");
        Assert.IsFalse (actual.Success);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: непустая строка")]
    public void Resolve_EscapedLiteral_5()
    {
        var actual = Resolve.EscapedLiteral().ParseOrThrow ("\"hello\"");
        Assert.AreEqual ("hello", actual);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: непустая строка с экранированным символом")]
    public void Resolve_EscapedLiteral_6()
    {
        var actual = Resolve.EscapedLiteral().ParseOrThrow ("\"hello\\nworld\"");
        Assert.AreEqual ("hello\\nworld", actual);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: непустая строка с экранированным символом-ограничителем")]
    public void Resolve_EscapedLiteral_7()
    {
        var actual = Resolve.EscapedLiteral().ParseOrThrow ("\"hello\\\"world\"");
        Assert.AreEqual ("hello\\\"world", actual);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: строка с неверно экранированным символом-ограничителем")]
    public void Resolve_EscapedLiteral_8()
    {
        var actual = Resolve.EscapedLiteral().Parse ("\"hello\\\"");
        Assert.IsFalse (actual.Success);
    }

    [TestMethod]
    [Description ("Строка с экранированными символами: строка с неверно экранированным символом-не-ограничителем")]
    public void Resolve_EscapedLiteral_9()
    {
        var actual = Resolve.EscapedLiteral().Parse ("\"hello\\n");
        Assert.IsFalse (actual.Success);
    }
}
