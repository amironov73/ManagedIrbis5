// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

namespace UnitTests.AM.Text;

[TestClass]
public sealed class StreamParserTest
    : Common.CommonUnitTest
{
    [TestMethod]
    [Description ("Конструирование")]
    public void StreamParser_Construction_1()
    {
        const string text = "Hello, world!";
        var reader = new StringReader (text);
        var parser = new StreamParser (reader);
        Assert.IsNotNull (parser);
        Assert.IsFalse (parser.EndOfStream);
    }

    [TestMethod]
    [Description ("Считывание из файла")]
    public void StreamParser_FromFile_1()
    {
        var fileName = Path.Combine
            (
                TestDataPath,
                "record.txt"
            );

        using var parser = StreamParser.FromFile (fileName, Encoding.UTF8);
        Assert.IsNotNull (parser);
        Assert.IsFalse (parser.EndOfStream);
    }

    [TestMethod]
    [Description ("Целое 16-битное число со знаком")]
    public void StreamParser_ReadInt16_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadInt16();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (1234, actual.Value);

        parser = StreamParser.FromString (" -1234 ");
        actual = parser.ReadInt16();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (-1234, actual.Value);

        parser = StreamParser.FromString ("  ");
        actual = parser.ReadInt16();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 16-битное число без знака")]
    public void StreamParser_ReadUInt16_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadUInt16();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (1234u, actual.Value);

        parser = StreamParser.FromString ("  ");
        actual = parser.ReadUInt16();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 32-битное число со знаком")]
    public void StreamParser_ReadInt32_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadInt32();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (1234, actual.Value);

        parser = StreamParser.FromString (" -1234 ");
        actual = parser.ReadInt32();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (-1234, actual.Value);

        parser = StreamParser.FromString ("  ");
        actual = parser.ReadInt32();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 32-битное число со знаком: неверный формат")]
    public void StreamParser_ReadInt32_2()
    {
        const string text = "  ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadInt32();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 32-битное число без знака")]
    public void StreamParser_ReadUInt32_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadUInt32();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (1234u, actual.Value);

        parser = StreamParser.FromString ("  ");
        actual = parser.ReadUInt32();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 64-битное число со знаком")]
    public void StreamParser_ReadInt64_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadInt64();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (1234, actual.Value);

        parser = StreamParser.FromString (" -1234 ");
        actual = parser.ReadInt64();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (-1234, actual.Value);

        parser = StreamParser.FromString ("  ");
        actual = parser.ReadInt64();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    public void StreamParser_ReadUInt64_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadUInt64();
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (1234u, actual.Value);


        parser = StreamParser.FromString ("  ");
        actual = parser.ReadUInt64();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 128-битное число со знаком")]
    public void StreamParser_ReadInt128_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadInt128();
        var expected = Int128.Parse ("1234");
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (expected, actual.Value);

        parser = StreamParser.FromString (" -1234 ");
        actual = parser.ReadInt128();
        expected = Int128.Parse ("-1234");
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (expected, actual.Value);

        parser = StreamParser.FromString ("  ");
        actual = parser.ReadInt128();
        Assert.IsFalse (actual.HasValue);
    }

    [TestMethod]
    [Description ("Целое 128-битное число без знака")]
    public void StreamParser_ReadUInt128_1()
    {
        const string text = "  \t1234 ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadUInt64();
        var expected = UInt128.Parse ("1234");
        Assert.IsTrue (actual.HasValue);
        Assert.AreEqual (expected, actual.Value);


        parser = StreamParser.FromString ("  ");
        actual = parser.ReadUInt64();
        Assert.IsFalse (actual.HasValue);
    }

    private void _TestDouble
        (
            string text,
            double expected
        )
    {
        var parser = StreamParser.FromString (text);
        var number = parser.ReadDouble();
        Assert.IsTrue (number.HasValue);
        Assert.AreEqual (expected, number.Value);
    }

    [TestMethod]
    [Description ("Число с плавающей точкой двойной точности")]
    public void StreamParser_ReadDouble_1()
    {
        _TestDouble ("1", 1.0);
        _TestDouble ("1.", 1.0);
        _TestDouble ("1.0", 1.0);
        _TestDouble ("+1", 1.0);
        _TestDouble ("-1", -1.0);
        _TestDouble ("1e2", 100.0);
        _TestDouble ("1e-2", 0.01);

        var parser = StreamParser.FromString ("  ");
        var number = parser.ReadDouble();
        Assert.IsFalse (number.HasValue);
    }

    [TestMethod]
    [Description ("Число с плавающей точкой двойной точности: ошибка формата")]
    public void StreamParser_ReadDouble_2()
    {
        const string text = "  ogo";
        var parser = StreamParser.FromString (text);
        var actual = parser.ReadDouble();
        Assert.IsFalse (actual.HasValue);
    }

    private void _TestSingle
        (
            string text,
            float expected
        )
    {
        var parser = StreamParser.FromString (text);
        var number = parser.ReadSingle();
        Assert.IsTrue (number.HasValue);
        Assert.AreEqual (expected, number.Value);
    }

    [TestMethod]
    [Description ("Число с плавающей точкой одинарной точности")]
    public void StreamParser_ReadSingle_1()
    {
        _TestSingle ("1", 1.0F);
        _TestSingle ("1.", 1.0F);
        _TestSingle ("1.0", 1.0F);
        _TestSingle ("+1", 1.0F);
        _TestSingle ("-1", -1.0F);
        _TestSingle ("1e2", 100.0F);
        _TestSingle ("1e-2", 0.01F);

        var parser = StreamParser.FromString ("  ");
        var number = parser.ReadSingle();
        Assert.IsFalse (number.HasValue);
    }

    private void _TestDecimal
        (
            string text,
            decimal expected
        )
    {
        var parser = StreamParser.FromString (text);
        var number = parser.ReadDecimal();
        Assert.IsTrue (number.HasValue);
        Assert.AreEqual (expected, number.Value);
    }

    [TestMethod]
    [Description ("Число с фиксированной точкой")]
    public void StreamParser_ReadDecimal_1()
    {
        _TestDecimal ("1", 1.0m);
        _TestDecimal ("1.", 1.0m);
        _TestDecimal ("1.0", 1.0m);
        _TestDecimal ("+1", 1.0m);
        _TestDecimal ("-1", -1.0m);

        var parser = StreamParser.FromString ("  ");
        var number = parser.ReadDecimal();
        Assert.IsFalse (number.HasValue);
    }


    [TestMethod]
    public void StreamParser_ReadSingle_2()
    {
        var parser = StreamParser.FromString (string.Empty);
        var number = parser.ReadDouble();
        Assert.IsFalse (number.HasValue);
    }

    [TestMethod]
    [Description ("Проверка на управляющие символы")]
    public void StreamParser_IsControl_1()
    {
        var parser = StreamParser.FromString ("1\nhello");
        Assert.IsFalse (parser.IsControl());
        parser.ReadChar();
        Assert.IsTrue (parser.IsControl());
        parser.ReadChar();
        Assert.IsFalse (parser.IsControl());
    }

    [TestMethod]
    [Description ("Проверка на букву")]
    public void StreamParser_IsLetter_1()
    {
        var parser = StreamParser.FromString ("1h!ello");
        Assert.IsFalse (parser.IsLetter());
        parser.ReadChar();
        Assert.IsTrue (parser.IsLetter());
        parser.ReadChar();
        Assert.IsFalse (parser.IsLetter());
    }

    [TestMethod]
    [Description ("Проверка: буква или цифра")]
    public void StreamParser_IsLetterOrDigit_1()
    {
        var parser = StreamParser.FromString ("1h!ello");
        Assert.IsTrue (parser.IsLetterOrDigit());
        parser.ReadChar();
        Assert.IsTrue (parser.IsLetterOrDigit());
        parser.ReadChar();
        Assert.IsFalse (parser.IsLetterOrDigit());
    }

    [TestMethod]
    [Description ("Проверка: числовой символ")]
    public void StreamParser_IsNumber_1()
    {
        var parser = StreamParser.FromString ("1½hello");
        Assert.IsTrue (parser.IsNumber());
        parser.ReadChar();
        Assert.IsTrue (parser.IsNumber());
        parser.ReadChar();
        Assert.IsFalse (parser.IsNumber());
    }

    [TestMethod]
    [Description ("Проверка на пунктуацию")]
    public void StreamParser_IsPunctuation_1()
    {
        var parser = StreamParser.FromString ("1!hello");
        Assert.IsFalse (parser.IsPunctuation());
        parser.ReadChar();
        Assert.IsTrue (parser.IsPunctuation());
        parser.ReadChar();
        Assert.IsFalse (parser.IsPunctuation());
    }

    [TestMethod]
    [Description ("Проверка на символ-разделитель")]
    public void StreamParser_IsSeparator_1()
    {
        var parser = StreamParser.FromString ("1 hello");
        Assert.IsFalse (parser.IsSeparator());
        parser.ReadChar();
        Assert.IsTrue (parser.IsSeparator());
        parser.ReadChar();
        Assert.IsFalse (parser.IsSeparator());
    }

    [TestMethod]
    [Description ("Проверка на Unicode-суррогат")]
    public void StreamParser_IsSurrogate_1()
    {
        var parser = StreamParser.FromString ("1\xd801hello");
        Assert.IsFalse (parser.IsSurrogate());
        parser.ReadChar();
        Assert.IsTrue (parser.IsSurrogate());
        parser.ReadChar();
        Assert.IsFalse (parser.IsSurrogate());
    }

    [TestMethod]
    [Description ("Проверка на спецсимволы")]
    public void StreamParser_IsSymbol_1()
    {
        var parser = StreamParser.FromString ("1№hello");
        Assert.IsFalse (parser.IsSymbol());
        parser.ReadChar();
        Assert.IsTrue (parser.IsSymbol());
        parser.ReadChar();
        Assert.IsFalse (parser.IsSymbol());
    }

    [TestMethod]
    public void StreamParser_SkipControl_1()
    {
        var parser = StreamParser.FromString ("\r\nhello");
        parser.SkipControl();
        Assert.AreEqual ('h', parser.ReadChar());
    }

    [TestMethod]
    public void StreamParser_SkipControl_2()
    {
        var parser = StreamParser.FromString ("\r\n");
        Assert.IsFalse (parser.SkipControl());
    }

    [TestMethod]
    [Description ("Пропуск пунктуации: есть данные после пунктуации")]
    public void StreamParser_SkipPunctuation_1()
    {
        var parser = StreamParser.FromString (".,hello");
        parser.SkipPunctuation();
        Assert.AreEqual ('h', parser.ReadChar());
    }

    [TestMethod]
    [Description ("Пропуск пунктуации: конец потока")]
    public void StreamParser_SkipPunctuation_2()
    {
        var parser = StreamParser.FromString (".,");
        Assert.IsFalse (parser.SkipPunctuation());
    }
}
