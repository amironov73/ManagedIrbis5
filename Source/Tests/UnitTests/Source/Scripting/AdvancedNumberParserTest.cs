// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Pidgin;
using static Pidgin.Parser<char>;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class AdvancedNumberParserTest
{
    [TestMethod]
    [Description ("Простейшие случаи: 32-битное целое со знаком")]
    public void AdvancedNumberParser_Parse_1()
    {
        var parser = Resolve.Int32().Before (End);
        Assert.AreEqual (0, parser.ParseOrThrow ("0"));
        Assert.AreEqual (123, parser.ParseOrThrow ("123"));
        Assert.AreEqual (-123, parser.ParseOrThrow ("-123"));
        Assert.AreEqual (123_456, parser.ParseOrThrow ("123_456"));
        Assert.AreEqual (-123_456, parser.ParseOrThrow ("-123_456"));
    }

    [TestMethod]
    [Description ("Простейшие случаи: 64-битное целое со знаком")]
    public void AdvancedNumberParser_Parse_2()
    {
        var parser = Resolve.Int64().Before (End);
        Assert.AreEqual (0L, parser.ParseOrThrow ("0"));
        Assert.AreEqual (123L, parser.ParseOrThrow ("123"));
        Assert.AreEqual (-123L, parser.ParseOrThrow ("-123"));
        Assert.AreEqual (123_456L, parser.ParseOrThrow ("123_456"));
        Assert.AreEqual (-123_456L, parser.ParseOrThrow ("-123_456"));
    }

    [TestMethod]
    [Description ("Простейшие случаи: 32-битное целое без знака")]
    public void AdvancedNumberParser_Parse_3()
    {
        var parser = Resolve.UInt32().Before (End);
        Assert.AreEqual (0U, parser.ParseOrThrow ("0"));
        Assert.AreEqual (123U, parser.ParseOrThrow ("123"));
        Assert.AreEqual (123_456U, parser.ParseOrThrow ("123_456"));
    }

    [TestMethod]
    [Description ("Простейшие случаи: 64-битное целое без знака")]
    public void AdvancedNumberParser_Parse_4()
    {
        var parser = Resolve.UInt64().Before (End);
        Assert.AreEqual (0UL, parser.ParseOrThrow ("0"));
        Assert.AreEqual (123UL, parser.ParseOrThrow ("123"));
        Assert.AreEqual (123_456UL, parser.ParseOrThrow ("123_456"));
    }

    [TestMethod]
    [Description ("Простейшие случаи: 32-битное целое без знака с суффиксом")]
    public void AdvancedNumberParser_Parse_5()
    {
        var suffixes = new[] { "u", "U" };
        var parser = Resolve.UInt32 (suffixes: suffixes).Before (End);
        Assert.AreEqual (0U, parser.ParseOrThrow ("0u"));
        Assert.AreEqual (0U, parser.ParseOrThrow ("0U"));
        Assert.AreEqual (123U, parser.ParseOrThrow ("123u"));
        Assert.AreEqual (123U, parser.ParseOrThrow ("123U"));
        Assert.AreEqual (123_456U, parser.ParseOrThrow ("123_456u"));
        Assert.AreEqual (123_456U, parser.ParseOrThrow ("123_456U"));
    }

    [TestMethod]
    [Description ("Простейшие случаи: 64-битное целое без знака с суффиксом")]
    public void AdvancedNumberParser_Parse_6()
    {
        var suffixes = new[] { "ul", "lu", "UL", "LU" };
        var parser = Resolve.UInt64 (suffixes: suffixes).Before (End);
        Assert.AreEqual (0UL, parser.ParseOrThrow ("0ul"));
        Assert.AreEqual (0UL, parser.ParseOrThrow ("0lu"));
        Assert.AreEqual (0UL, parser.ParseOrThrow ("0UL"));
        Assert.AreEqual (0UL, parser.ParseOrThrow ("0LU"));
        Assert.AreEqual (123UL, parser.ParseOrThrow ("123ul"));
        Assert.AreEqual (123UL, parser.ParseOrThrow ("123lu"));
        Assert.AreEqual (123UL, parser.ParseOrThrow ("123UL"));
        Assert.AreEqual (123UL, parser.ParseOrThrow ("123LU"));
        Assert.AreEqual (123_456UL, parser.ParseOrThrow ("123_456ul"));
        Assert.AreEqual (123_456UL, parser.ParseOrThrow ("123_456lu"));
        Assert.AreEqual (123_456UL, parser.ParseOrThrow ("123_456UL"));
        Assert.AreEqual (123_456UL, parser.ParseOrThrow ("123_456LU"));
    }

    [TestMethod]
    [Description ("Повторяющиеся знаки подчеркивания")]
    public void AdvancedNumberParser_Parse_7()
    {
        var parser = Resolve.Int32().Before (End);
        Assert.AreEqual (123, parser.ParseOrThrow ("1_2_3"));
        Assert.AreEqual (123, parser.ParseOrThrow ("1__2__3"));
        Assert.AreEqual (123, parser.ParseOrThrow ("123_"));
        Assert.AreEqual (123, parser.ParseOrThrow ("123__"));
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void AdvancedNumberParser_Parse_8()
    {
        var parser = Resolve.Int32().Before (End);
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow (string.Empty));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("-"));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("+"));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("_"));
    }

    [TestMethod]
    [Description ("Неверно сформированное число: подчеркивание")]
    public void AdvancedNumberParser_Parse_9()
    {
        var parser = Resolve.Int32().Before (End);
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("_1"));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("_-1"));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("_+1"));
    }

    [TestMethod]
    [Description ("Неверно сформированное число: система счисления")]
    public void AdvancedNumberParser_Parse_10()
    {
        var parser = Resolve.Int32 (2).Before (End);
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("123"));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("-123"));
        Assert.ThrowsException<ParseException> (() => parser.ParseOrThrow ("+123"));
    }

    [TestMethod]
    [Description ("Верно сформированное число: система счисления")]
    public void AdvancedNumberParser_Parse_11()
    {
        var parser = Resolve.Int32 (2).Before (End);
        Assert.AreEqual (0, parser.ParseOrThrow ("0"));
        Assert.AreEqual (1, parser.ParseOrThrow ("1"));
        Assert.AreEqual (2, parser.ParseOrThrow ("10"));
        Assert.AreEqual (3, parser.ParseOrThrow ("11"));
        Assert.AreEqual (4, parser.ParseOrThrow ("100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("-0"));
        Assert.AreEqual (-1, parser.ParseOrThrow ("-1"));
        Assert.AreEqual (-2, parser.ParseOrThrow ("-10"));
        Assert.AreEqual (-3, parser.ParseOrThrow ("-11"));
        Assert.AreEqual (-4, parser.ParseOrThrow ("-100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("+0"));
        Assert.AreEqual (1, parser.ParseOrThrow ("+1"));
        Assert.AreEqual (2, parser.ParseOrThrow ("+10"));
        Assert.AreEqual (3, parser.ParseOrThrow ("+11"));
        Assert.AreEqual (4, parser.ParseOrThrow ("+100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("000"));
        Assert.AreEqual (1, parser.ParseOrThrow ("001"));
        Assert.AreEqual (2, parser.ParseOrThrow ("010"));
        Assert.AreEqual (3, parser.ParseOrThrow ("011"));
        Assert.AreEqual (4, parser.ParseOrThrow ("100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("0_000"));
        Assert.AreEqual (1, parser.ParseOrThrow ("0_001"));
        Assert.AreEqual (2, parser.ParseOrThrow ("0_010"));
        Assert.AreEqual (3, parser.ParseOrThrow ("0_011"));
        Assert.AreEqual (4, parser.ParseOrThrow ("0_100"));
    }

    [TestMethod]
    [Description ("Верно сформированное число: шестнадцатеричное")]
    public void AdvancedNumberParser_Parse_12()
    {
        var parser = Resolve.Int32 (16, "0x").Before (End);
        Assert.AreEqual (0, parser.ParseOrThrow ("0x0"));
        Assert.AreEqual (1, parser.ParseOrThrow ("0x1"));
        Assert.AreEqual (16, parser.ParseOrThrow ("0x10"));
        Assert.AreEqual (17, parser.ParseOrThrow ("0x11"));
        Assert.AreEqual (256, parser.ParseOrThrow ("0x100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("-0x0"));
        Assert.AreEqual (-1, parser.ParseOrThrow ("-0x1"));
        Assert.AreEqual (-16, parser.ParseOrThrow ("-0x10"));
        Assert.AreEqual (-17, parser.ParseOrThrow ("-0x11"));
        Assert.AreEqual (-256, parser.ParseOrThrow ("-0x100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("+0x0"));
        Assert.AreEqual (1, parser.ParseOrThrow ("+0x1"));
        Assert.AreEqual (16, parser.ParseOrThrow ("+0x10"));
        Assert.AreEqual (17, parser.ParseOrThrow ("+0x11"));
        Assert.AreEqual (256, parser.ParseOrThrow ("+0x100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("0x000"));
        Assert.AreEqual (1, parser.ParseOrThrow ("0x001"));
        Assert.AreEqual (16, parser.ParseOrThrow ("0x010"));
        Assert.AreEqual (17, parser.ParseOrThrow ("0x011"));
        Assert.AreEqual (256, parser.ParseOrThrow ("0x100"));

        Assert.AreEqual (0, parser.ParseOrThrow ("0x0_000"));
        Assert.AreEqual (1, parser.ParseOrThrow ("0x0_001"));
        Assert.AreEqual (16, parser.ParseOrThrow ("0x0_010"));
        Assert.AreEqual (17, parser.ParseOrThrow ("0x0_011"));
        Assert.AreEqual (256, parser.ParseOrThrow ("0x0_100"));
    }

    [TestMethod]
    [Description ("Суффикс нужен, но его нет")]
    public void AdvancedNumberParser_Parse_13()
    {
        var suffixes = new[] { "ul", "lu", "UL", "LU" };
        var parser = Resolve.UInt64 (suffixes: suffixes).Before (End);
        Assert.IsFalse (parser.Parse (string.Empty).Success);
        Assert.IsFalse (parser.Parse ("1").Success);
        Assert.IsFalse (parser.Parse ("-1").Success);
        Assert.IsFalse (parser.Parse ("1_000").Success);
        Assert.IsTrue (parser.Parse ("1_000ul").Success);
    }

    [TestMethod]
    [Description ("Целое число произвольной длины: верно сформированное")]
    public void AdvancedNumberParser_Parse_14()
    {
        var suffixes = new[] { "b", "B" };
        var parser = Resolve.BigInteger (suffixes).Before (End);
        Assert.AreEqual (new BigInteger (1_000_000), parser.ParseOrThrow ("1_000_000b"));
        Assert.AreEqual (new BigInteger (1_000_000), parser.ParseOrThrow ("1_000_000B"));
        Assert.AreEqual (new BigInteger (-1_000_000), parser.ParseOrThrow ("-1_000_000b"));
        Assert.AreEqual (new BigInteger (-1_000_000), parser.ParseOrThrow ("-1_000_000B"));

        Assert.AreEqual (new BigInteger (1_000_000_000L), parser.ParseOrThrow ("1_000_000_000b"));
        Assert.AreEqual (new BigInteger (1_000_000_000L), parser.ParseOrThrow ("1_000_000_000B"));
        Assert.AreEqual (new BigInteger (-1_000_000_000L), parser.ParseOrThrow ("-1_000_000_000b"));
        Assert.AreEqual (new BigInteger (-1_000_000_000L), parser.ParseOrThrow ("-1_000_000_000B"));
    }

    [TestMethod]
    [Description ("Целое число произвольной длины: неверно сформированное")]
    public void AdvancedNumberParser_Parse_15()
    {
        var suffixes = new[] { "b", "B" };
        var parser = Resolve.BigInteger (suffixes).Before (End);
        Assert.IsFalse (parser.Parse ("1_000_000").Success);
        Assert.IsFalse (parser.Parse ("-1_000_000").Success);

        Assert.IsFalse (parser.Parse ("1_000_000A").Success);
        Assert.IsFalse (parser.Parse ("-1_000_000a").Success);
    }

    [TestMethod]
    [Description ("Подчеркивание сразу вслед за префиксом")]
    public void AdvancedNumberParser_Parse_16()
    {
        var parser = Resolve.Int32 (16, "0x").Before (End);
        Assert.AreEqual (0, parser.ParseOrThrow ("0x_0"));
        Assert.AreEqual (1, parser.ParseOrThrow ("0x_1"));
        Assert.AreEqual (16, parser.ParseOrThrow ("0x_10"));
        Assert.AreEqual (17, parser.ParseOrThrow ("0x_11"));
        Assert.AreEqual (256, parser.ParseOrThrow ("0x_100"));
    }
}
