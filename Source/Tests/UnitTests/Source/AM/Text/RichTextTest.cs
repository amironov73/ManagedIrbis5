// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTests.AM.Text;

[TestClass]
public sealed class RichTextTest
{
    [TestMethod]
    [Description ("Статический конструктор")]
    public void RichText_Construction_1()
    {
        Assert.IsNotNull (RichText.CentralEuropeanPrologue);
        Assert.IsNotNull (RichText.CommonPrologue);
        Assert.IsNotNull (RichText.RussianPrologue);
        Assert.IsNotNull (RichText.WesternEuropeanPrologue);
    }

    [TestMethod]
    [Description ("Баланс фигурных скобок")]
    public void RichText_CheckBraces_1()
    {
        Assert.IsTrue (RichText.CheckBraces (ReadOnlySpan<char>.Empty));
        Assert.IsTrue (RichText.CheckBraces (" "));
        Assert.IsTrue (RichText.CheckBraces ("\r\n"));

        Assert.IsTrue (RichText.CheckBraces ("{}"));
        Assert.IsTrue (RichText.CheckBraces ("{hello}"));
        Assert.IsTrue (RichText.CheckBraces ("hello{}world"));
        Assert.IsTrue (RichText.CheckBraces ("{{}}"));
        Assert.IsTrue (RichText.CheckBraces ("{hello{}world}"));
        Assert.IsTrue (RichText.CheckBraces ("{}{}"));
        Assert.IsTrue (RichText.CheckBraces ("{hello}{world}"));
        Assert.IsTrue (RichText.CheckBraces ("{}{}{}"));
        Assert.IsTrue (RichText.CheckBraces ("{}hello{}world{}"));

        Assert.IsTrue (RichText.CheckBraces (@"\{"));
        Assert.IsTrue (RichText.CheckBraces (@"\{hello"));
        Assert.IsTrue (RichText.CheckBraces (@"hello\{"));
        Assert.IsTrue (RichText.CheckBraces (@"\}"));
        Assert.IsTrue (RichText.CheckBraces (@"\}hello"));
        Assert.IsTrue (RichText.CheckBraces (@"hello\}"));
        Assert.IsTrue (RichText.CheckBraces (@"\{\}"));
        Assert.IsTrue (RichText.CheckBraces (@"hello\{\}"));
        Assert.IsTrue (RichText.CheckBraces (@"\{hello\}"));
        Assert.IsTrue (RichText.CheckBraces (@"\{\}hello"));
        Assert.IsTrue (RichText.CheckBraces (@"{\}}"));

        Assert.IsFalse (RichText.CheckBraces ("{"));
        Assert.IsFalse (RichText.CheckBraces ("}"));
        Assert.IsFalse (RichText.CheckBraces ("}{"));
        Assert.IsFalse (RichText.CheckBraces ("{}{"));
        Assert.IsFalse (RichText.CheckBraces ("{}}"));

        Assert.IsFalse (RichText.CheckBraces (@"{}\"));
        Assert.IsFalse (RichText.CheckBraces (@"\"));
    }

    [TestMethod]
    [Description ("Декодирование RTF-разметки: только латиница")]
    public void RichText_Decode_1()
    {
        Assert.AreEqual
            (
                null, RichText.Decode (null)
            );

        Assert.AreEqual
            (
                string.Empty,
                RichText.Decode (string.Empty)
            );

        Assert.AreEqual
            (
                "\\",
                RichText.Decode ("\\")
            );

        Assert.AreEqual
            (
                "Hello",
                RichText.Decode ("Hello")
            );

        Assert.AreEqual
            (
                "\\{\\}",
                RichText.Decode ("\\{\\}")
            );

        Assert.AreEqual
            (
                "\\b Hello \\b0",
                RichText.Decode ("\\b Hello \\b0")
            );
    }

    [TestMethod]
    [Description ("Декодирование RTF-разметки: кириллица")]
    public void RichText_Decode_2()
    {
        Assert.AreEqual
            (
                "Привет",
                RichText.Decode ("\\u1055?\\u1088?\\u1080?\\u1074?\\u1077?\\u1090?")
            );
    }

    [TestMethod]
    [Description ("Кодирование в RTF: только латиница")]
    public void RichText_Encode_1()
    {
        Assert.AreEqual
            (
                null,
                RichText.Encode (null, null)
            );

        Assert.AreEqual
            (
                "",
                RichText.Encode ("", null)
            );

        Assert.AreEqual
            (
                "Hello",
                RichText.Encode ("Hello", null)
            );

        Assert.AreEqual
            (
                "\\{\\}",
                RichText.Encode ("{}", null)
            );

        Assert.AreEqual
            (
                "Hel\\\\lo",
                RichText.Encode ("Hel\\lo", null)
            );
    }

    [TestMethod]
    [Description ("Кодирование в RTF: символы")]
    public void RichText_Encode_2()
    {
        Assert.AreEqual
            (
                "Hello,\\'0aWorld!",
                RichText.Encode ("Hello,\nWorld!", null)
            );

        Assert.AreEqual
            (
                "Copyright \\'a9 2017",
                RichText.Encode ("Copyright © 2017", null)
            );
    }

    [TestMethod]
    [Description ("Кодирование в RTF: кириллица не перекодируется")]
    public void RichText_Encode_3()
    {
        var range = UnicodeRange.Cyrillic;

        Assert.AreEqual
            (
                "Hello",
                RichText.Encode ("Hello", range)
            );

        Assert.AreEqual
            (
                "Привет",
                RichText.Encode ("Привет", range)
            );
    }

    [TestMethod]
    [Description ("Кодирование в RTF: кириллица перекодируется")]
    public void RichText_Encode_4()
    {
        var range = UnicodeRange.LatinExtended;

        Assert.AreEqual
            (
                "\\u1055?\\u1088?\\u1080?\\u1074?\\u1077?\\u1090?",
                RichText.Encode ("Привет", range)
            );
    }

    [TestMethod]
    [Description ("Кодирование в RTF: кириллица не перекодируется")]
    public void RichText_Encode_5()
    {
        var range = UnicodeRange.Russian;

        Assert.AreEqual
            (
                null,
                RichText.Encode2 (null, range)
            );

        Assert.AreEqual
            (
                String.Empty,
                RichText.Encode2 (string.Empty, range)
            );

        Assert.AreEqual
            (
                "\\'09",
                RichText.Encode2 ("\t", range)
            );

        Assert.AreEqual
            (
                "Привет",
                RichText.Encode2 ("Привет", range)
            );

        Assert.AreEqual
            (
                "\\u12371?\\u12435?\\u12395?\\u12385?\\u12399?",
                RichText.Encode2 ("こんにちは", range)
            );

        Assert.AreEqual
            (
                "Geld f\\'fcr Maria : erz\\'e4hlung",
                RichText.Encode2 ("Geld für Maria : erzählung", range)
            );
    }

    [TestMethod]
    [Description ("Кодирование в RTF: переключение на шрифт")]
    public void RichText_Encode_6()
    {
        var range = UnicodeRange.Russian;
        const string fontSwitch = "\\f2";

        Assert.AreEqual
            (
                null,
                RichText.Encode3 (null, range, fontSwitch)
            );

        Assert.AreEqual
            (
                string.Empty,
                RichText.Encode3 (string.Empty, range, fontSwitch)
            );

        Assert.AreEqual
            (
                "\\'09",
                RichText.Encode3 ("\t", range, fontSwitch)
            );

        Assert.AreEqual
            (
                "Привет",
                RichText.Encode3 ("Привет", range, fontSwitch)
            );

        Assert.AreEqual
            (
                "\\u12371?\\u12435?\\u12395?\\u12385?\\u12399?",
                RichText.Encode3 ("こんにちは", range, fontSwitch)
            );

        Assert.AreEqual
            (
                "Geld f{\\f2\\'fc}r Maria : erz{\\f2\\'e4}hlung",
                RichText.Encode3 ("Geld für Maria : erzählung", range, fontSwitch)
            );
    }
}
