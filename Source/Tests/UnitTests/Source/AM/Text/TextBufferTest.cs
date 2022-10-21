// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using System;
using System.Text;

using AM.Text;

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public sealed class TextBufferTest
{
    static string Replicate (string text, int count)
    {
        var result = new StringBuilder (text.Length * count);
        for (var i = 0; i < count; i++)
        {
            result.Append (text);
        }

        return result.ToString();
    }

    [TestMethod]
    public void TextBuffer_Construction_1()
    {
        var buffer = new TextBuffer();
        Assert.AreEqual (1, buffer.Column);
        Assert.AreEqual (1, buffer.Line);
        Assert.AreEqual (0, buffer.Length);
    }

    [TestMethod]
    public void TextBuffer_Backspace_1()
    {
        var buffer = new TextBuffer();
        Assert.IsFalse (buffer.Backspace());

        buffer.Write ("Hello");
        Assert.IsTrue (buffer.Backspace());
        Assert.AreEqual ("Hell", buffer.ToString());
        Assert.AreEqual (5, buffer.Column);
        Assert.AreEqual (1, buffer.Line);
    }

    [TestMethod]
    public void TextBuffer_Backspace_2()
    {
        var buffer = new TextBuffer();
        Assert.IsFalse (buffer.Backspace());

        buffer.Write ("Hello\n");
        Assert.IsTrue (buffer.Backspace());
        Assert.AreEqual ("Hello", buffer.ToString());
        Assert.AreEqual (6, buffer.Column);
        Assert.AreEqual (1, buffer.Line);
    }

    [TestMethod]
    public void TextBuffer_Backspace_3()
    {
        var buffer = new TextBuffer();
        Assert.IsFalse (buffer.Backspace());

        buffer.Write ("Hello\nworld\n");
        Assert.IsTrue (buffer.Backspace());
        Assert.AreEqual ("Hello\nworld", buffer.ToString());
        Assert.AreEqual (6, buffer.Column);
        Assert.AreEqual (2, buffer.Line);
    }

    [TestMethod]
    public void TextBuffer_Clear_1()
    {
        var buffer = new TextBuffer();
        buffer.Write ("Hello");

        Assert.AreSame (buffer, buffer.Clear());
        Assert.AreEqual (1, buffer.Line);
        Assert.AreEqual (1, buffer.Column);
        Assert.AreEqual (0, buffer.Length);
    }

    [TestMethod]
    public void TextBuffer_GetLastChar_1()
    {
        var buffer = new TextBuffer();
        Assert.AreEqual ('\0', buffer.GetLastChar());

        buffer.Write ("Hello");
        Assert.AreEqual ('o', buffer.GetLastChar());
    }

    [TestMethod]
    public void TextBuffer_PrecededByNewLine_1()
    {
        var buffer = new TextBuffer();
        Assert.IsFalse (buffer.PrecededByNewLine());

        buffer.Write ("Hello");
        Assert.IsFalse (buffer.PrecededByNewLine());

        buffer.WriteLine();
        Assert.IsTrue (buffer.PrecededByNewLine());

        buffer.Write ("World");
        Assert.IsFalse (buffer.PrecededByNewLine());
    }

    [TestMethod]
    public void TextBuffer_RemoveEmptyLines_1()
    {
        var buffer = new TextBuffer();
        Assert.AreSame (buffer, buffer.RemoveEmptyLines());

        buffer.Write ("Hello");
        buffer.WriteLine();
        buffer.RemoveEmptyLines();
        Assert.AreEqual ("Hello", buffer.ToString());

        buffer.WriteLine();
        buffer.WriteLine();
        buffer.RemoveEmptyLines();
        Assert.AreEqual ("Hello", buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_Write_1()
    {
        var buffer = new TextBuffer();
        var length = 10 * 1024;
        var c = 'A';
        var expected = new string (c, length);
        for (var i = 0; i < length; i++)
        {
            Assert.AreSame (buffer, buffer.Write (c));
        }

        Assert.AreEqual (length, buffer.Length);
        Assert.AreEqual (length + 1, buffer.Column);
        Assert.AreEqual (1, buffer.Line);
        Assert.AreEqual (expected, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_Write_2()
    {
        var buffer = new TextBuffer();
        var length = 10 * 1024;
        var c = '\n';
        var expected = new string (c, length);
        for (var i = 0; i < length; i++)
        {
            Assert.AreSame (buffer, buffer.Write (c));
        }

        Assert.AreEqual (length, buffer.Length);
        Assert.AreEqual (1, buffer.Column);
        Assert.AreEqual (length + 1, buffer.Line);
        Assert.AreEqual (expected, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_Write_3()
    {
        var buffer = new TextBuffer();
        var one = "one";
        var length = 1000;
        var total = length * one.Length;
        for (var i = 0; i < length; i++)
        {
            Assert.AreSame (buffer, buffer.Write (one));
        }

        Assert.AreEqual (total, buffer.Length);
        Assert.AreEqual (total + 1, buffer.Column);
        Assert.AreEqual (1, buffer.Line);
        string expected = Replicate (one, length);
        Assert.AreEqual (expected, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_Write_4()
    {
        var buffer = new TextBuffer();
        var one = "o\ne";
        var length = 1000;
        var total = length * one.Length;
        for (var i = 0; i < length; i++)
        {
            Assert.AreSame (buffer, buffer.Write (one));
        }

        Assert.AreEqual (total, buffer.Length);
        Assert.AreEqual (2, buffer.Column);
        Assert.AreEqual (length + 1, buffer.Line);
        string expected = Replicate (one, length);
        Assert.AreEqual (expected, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_Write_5()
    {
        var buffer = new TextBuffer();
        Assert.AreSame (buffer, buffer.Write (null));
        Assert.AreEqual (0, buffer.Length);
        Assert.AreEqual (1, buffer.Line);
        Assert.AreEqual (1, buffer.Column);

        Assert.AreSame (buffer, buffer.Write (string.Empty));
        Assert.AreEqual (0, buffer.Length);
        Assert.AreEqual (1, buffer.Line);
        Assert.AreEqual (1, buffer.Column);
    }

    [TestMethod]
    public void TextBuffer_Write_6()
    {
        var buffer = new TextBuffer();
        Assert.AreSame (buffer, buffer.Write ("{0}, {1}!", "Hello", "world"));
        Assert.AreEqual ("Hello, world!", buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_WriteLine_1()
    {
        var buffer = new TextBuffer();
        Assert.AreSame (buffer, buffer.WriteLine());
        Assert.AreEqual (Environment.NewLine, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_WriteLine_2()
    {
        var buffer = new TextBuffer();
        var text = "Hello";
        Assert.AreSame (buffer, buffer.WriteLine (text));
        Assert.AreEqual (text + Environment.NewLine, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_WriteLine_3()
    {
        var buffer = new TextBuffer();
        Assert.AreSame (buffer, buffer.WriteLine ("{0}, {1}!", "Hello", "world"));
        Assert.AreEqual ("Hello, world!" + Environment.NewLine, buffer.ToString());
    }

    [TestMethod]
    public void TextBuffer_ToString_1()
    {
        var buffer = new TextBuffer();
        Assert.AreEqual (string.Empty, buffer.ToString());

        var text = "Hello, world";
        buffer.Write (text);
        Assert.AreEqual (text, buffer.ToString());
    }
}
