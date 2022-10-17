// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Text;

using AM;
using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTests.AM.IO;

[TestClass]
public sealed class UtfReaderTest
{
    private static void _CountChars (string text)
    {
        var expected = text.Length;
        var bytes = Encoding.UTF8.GetBytes (text);
        var reader = new UtfReader (bytes);
        var actual = reader.CountChars();

        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void UtfReader_CountChars_1()
    {
        _CountChars (string.Empty);
        _CountChars ("Hello");
        _CountChars ("Привет");
        _CountChars ("こんにちは");
    }

    [TestMethod]
    public void UtfReader_IsControl_1()
    {
        var reader = new UtfReader ("H\tllo");
        Assert.IsFalse (reader.IsControl());
        reader.ReadChar();
        Assert.IsTrue (reader.IsControl());
        reader.ReadChar();
        Assert.IsFalse (reader.IsControl());
    }

    [TestMethod]
    public void UtfReader_IsDigit_1()
    {
        var reader = new UtfReader ("H1llo");
        Assert.IsFalse (reader.IsDigit());
        reader.ReadChar();
        Assert.IsTrue (reader.IsDigit());
        reader.ReadChar();
        Assert.IsFalse (reader.IsDigit());
    }

    [TestMethod]
    public void UtfReader_IsLetter_1()
    {
        var reader = new UtfReader ("1e2lo");
        Assert.IsFalse (reader.IsLetter());
        reader.ReadChar();
        Assert.IsTrue (reader.IsLetter());
        reader.ReadChar();
        Assert.IsFalse (reader.IsLetter());
    }

    [TestMethod]
    public void UtfReader_IsLetterOrDigit_1()
    {
        var reader = new UtfReader ("_e_");
        Assert.IsFalse (reader.IsLetterOrDigit());
        reader.ReadChar();
        Assert.IsTrue (reader.IsLetterOrDigit());
        reader.ReadChar();
        Assert.IsFalse (reader.IsLetterOrDigit());
    }

    [TestMethod]
    public void UtfReader_IsLetterOrDigit_2()
    {
        var reader = new UtfReader ("_1_");
        Assert.IsFalse (reader.IsLetterOrDigit());
        reader.ReadChar();
        Assert.IsTrue (reader.IsLetterOrDigit());
        reader.ReadChar();
        Assert.IsFalse (reader.IsLetterOrDigit());
    }

    [TestMethod]
    public void UtfReader_IsNumber_1()
    {
        var reader = new UtfReader ("_1_");
        Assert.IsFalse (reader.IsNumber());
        reader.ReadChar();
        Assert.IsTrue (reader.IsNumber());
        reader.ReadChar();
        Assert.IsFalse (reader.IsNumber());
    }

    [TestMethod]
    public void UtfReader_IsPunctuation_1()
    {
        var reader = new UtfReader ("1.2");
        Assert.IsFalse (reader.IsPunctuation());
        reader.ReadChar();
        Assert.IsTrue (reader.IsPunctuation());
        reader.ReadChar();
        Assert.IsFalse (reader.IsPunctuation());
    }

    [TestMethod]
    public void UtfReader_IsSeparator_1()
    {
        var reader = new UtfReader ("1 2");
        Assert.IsFalse (reader.IsSeparator());
        reader.ReadChar();
        Assert.IsTrue (reader.IsSeparator());
        reader.ReadChar();
        Assert.IsFalse (reader.IsSeparator());
    }

    [TestMethod]
    public void UtfReader_IsSymbol_1()
    {
        var reader = new UtfReader ("1+2");
        Assert.IsFalse (reader.IsSymbol());
        reader.ReadChar();
        Assert.IsTrue (reader.IsSymbol());
        reader.ReadChar();
        Assert.IsFalse (reader.IsSymbol());
    }

    [TestMethod]
    public void UtfReader_IsWhitespace_1()
    {
        var reader = new UtfReader ("1 2");
        Assert.IsFalse (reader.IsWhiteSpace());
        reader.ReadChar();
        Assert.IsTrue (reader.IsWhiteSpace());
        reader.ReadChar();
        Assert.IsFalse (reader.IsWhiteSpace());
    }

    [TestMethod]
    public void UtfReader_ReadChar_1()
    {
        var reader = new UtfReader ("Hello");
        Assert.IsFalse (reader.IsEOF);
        Assert.AreEqual ('H', reader.ReadChar());
        Assert.AreEqual ('e', reader.ReadChar());
        Assert.AreEqual ('l', reader.ReadChar());
        Assert.AreEqual ('l', reader.ReadChar());
        Assert.AreEqual ('o', reader.ReadChar());
        Assert.IsTrue (reader.IsEOF);
        Assert.AreEqual ('\0', reader.ReadChar());
        Assert.IsTrue (reader.IsEOF);
    }

    [TestMethod]
    public void UtfReader_ReadChar_2()
    {
        var reader = new UtfReader ("Привет");
        Assert.IsFalse (reader.IsEOF);
        Assert.AreEqual ('П', reader.ReadChar());
        Assert.AreEqual ('р', reader.ReadChar());
        Assert.AreEqual ('и', reader.ReadChar());
        Assert.AreEqual ('в', reader.ReadChar());
        Assert.AreEqual ('е', reader.ReadChar());
        Assert.AreEqual ('т', reader.ReadChar());
        Assert.IsTrue (reader.IsEOF);
        Assert.AreEqual ('\0', reader.ReadChar());
        Assert.IsTrue (reader.IsEOF);
    }

    [TestMethod]
    public void UtfReader_ReadChar_3()
    {
        var reader = new UtfReader ("こんにちは");
        Assert.IsFalse (reader.IsEOF);
        Assert.AreEqual ('こ', reader.ReadChar());
        Assert.AreEqual ('ん', reader.ReadChar());
        Assert.AreEqual ('に', reader.ReadChar());
        Assert.AreEqual ('ち', reader.ReadChar());
        Assert.AreEqual ('は', reader.ReadChar());
        Assert.IsTrue (reader.IsEOF);
        Assert.AreEqual ('\0', reader.ReadChar());
        Assert.IsTrue (reader.IsEOF);
    }

    [TestMethod]
    [Description ("Суррогаты не обрабатываются")]
    [ExpectedException (typeof (OverflowException))]
    public void UtfReader_ReadChar_4()
    {
        var reader = new UtfReader ("\U00010F00");
        _ = reader.ReadChar();
    }

    private void _ReadTo (string expected, string text, char stop)
    {
        var reader = new UtfReader (text);

        Assert.IsTrue
            (
                Utility.CompareSpans
                    (
                        Encoding.UTF8.GetBytes (expected),
                        reader.ReadTo (stop)
                    )
                    == 0
            );
    }

    [TestMethod]
    public void UtfReader_ReadTo_1()
    {
        _ReadTo ("Hello", "Hello, world", ',');
        _ReadTo ("world", "world", ',');

        _ReadTo ("Привет", "Привет, мир", ',');
        _ReadTo ("мир", "мир", ',');
    }

    private void _ReadByteLine (string expected, string text)
    {
        var reader = new UtfReader (text);

        Assert.IsTrue
            (
                Utility.CompareSpans
                    (
                        Encoding.UTF8.GetBytes (expected),
                        reader.ReadByteLine()
                    )
                == 0
            );
    }

    [TestMethod]
    public void UtfReader_ReadByteLine_1()
    {
        _ReadByteLine ("Hello", "Hello\nworld");
        _ReadByteLine ("Hello", "Hello\r\nworld");
        _ReadByteLine ("Hello", "Hello\rworld");
        _ReadByteLine ("Hello", "Hello");

        _ReadByteLine ("Привет", "Привет\nмир");
        _ReadByteLine ("Привет", "Привет\r\nмир");
        _ReadByteLine ("Привет", "Привет\rмир");
        _ReadByteLine ("Привет", "Привет");
    }

    private void _SkipLine (string expected, string text)
    {
        var reader = new UtfReader (text);

        reader.SkipLine();
        Assert.IsTrue
            (
                Utility.CompareSpans
                    (
                        Encoding.UTF8.GetBytes (expected),
                        reader.Remaining
                    )
                == 0
            );
    }

    [TestMethod]
    public void UtfReader_SkipLine_1()
    {
        _SkipLine ("world", "Hello\nworld");
        _SkipLine ("world", "Hello\r\nworld");
        _SkipLine ("world", "Hello\rworld");
        _SkipLine (string.Empty, "Hello");

        _SkipLine ("мир", "Привет\nмир");
        _SkipLine ("мир", "Привет\r\nмир");
        _SkipLine ("мир", "Привет\rмир");
        _SkipLine (string.Empty, "Привет");
    }

    private void _ReadLine (string expected, string text)
    {
        var reader = new UtfReader (text);

        Assert.IsTrue
            (
                string.CompareOrdinal
                    (
                        expected,
                        reader.ReadLine()
                    )
                == 0
            );
    }

    [TestMethod]
    public void UtfReader_ReadLine_1()
    {
        _ReadLine ("Hello", "Hello\nworld");
        _ReadLine ("Hello", "Hello\r\nworld");
        _ReadLine ("Hello", "Hello\rworld");
        _ReadLine ("Hello", "Hello");

        _ReadLine ("Привет", "Привет\nмир");
        _ReadLine ("Привет", "Привет\r\nмир");
        _ReadLine ("Привет", "Привет\rмир");
        _ReadLine ("Привет", "Привет");
    }
}
